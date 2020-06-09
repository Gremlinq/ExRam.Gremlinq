using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertyModel
    {
        private sealed class GraphElementPropertyModelImpl : IGraphElementPropertyModel
        {
            public GraphElementPropertyModelImpl(IImmutableDictionary<MemberInfo, MemberMetadata> metadata, IImmutableDictionary<MemberInfo, T> specialNames)
            {
                MemberMetadata = metadata;
                SpecialNames = specialNames;
            }
            
            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    transformation(MemberMetadata),
                    SpecialNames);
            }

            public IGraphElementPropertyModel ConfigureSpecialNames(Func<IImmutableDictionary<MemberInfo, T>, IImmutableDictionary<MemberInfo, T>> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    MemberMetadata,
                    transformation(SpecialNames));
            }

            public IImmutableDictionary<MemberInfo, T> SpecialNames { get; }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MemberMetadata { get; }
        }

        private sealed class KeyLookup
        {
            private static readonly Dictionary<string, T> DefaultTs = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase)
            {
                { "id", T.Id },
                { "label", T.Label }
            };

            private readonly HashSet<T> _configuredTs;
            private readonly IGraphElementPropertyModel _model;
            private readonly ConcurrentDictionary<MemberInfo, object> _members = new ConcurrentDictionary<MemberInfo, object>();

            public KeyLookup(IGraphElementPropertyModel model)
            {
                _model = model;
                _configuredTs = new HashSet<T>(model.SpecialNames
                    .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
                    .Keys);
            }

            public object GetKey(MemberInfo member)
            {
                return _members.GetOrAdd(
                    member,
                    (closureMember, closureModel) =>
                    {
                        if (closureModel.SpecialNames.TryGetValue(closureMember, out var specialName))
                            return specialName;

                        if (DefaultTs.TryGetValue(closureMember.Name, out var defaultT) && !_configuredTs.Contains(defaultT))
                            return defaultT;

                        return closureModel.MemberMetadata.TryGetValue(closureMember, out var metadata)
                            ? metadata.Name
                            : closureMember.Name;
                    },
                    _model);
            }
        }

        public static readonly IGraphElementPropertyModel Empty = new GraphElementPropertyModelImpl(
            ImmutableDictionary<MemberInfo, MemberMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance),
            ImmutableDictionary<MemberInfo, T>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance));

        private static readonly ConditionalWeakTable<IGraphElementPropertyModel, KeyLookup> IdentifierDict = new ConditionalWeakTable<IGraphElementPropertyModel, KeyLookup>();

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IMemberMetadataConfigurator<TElement>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            where TElement : class
        {
            return model.ConfigureMemberMetadata(
                metadata => transformation(new MemberMetadataConfigurator<TElement>(metadata)));
        }

        internal static object GetKey(this IGraphElementPropertyModel model, Expression expression)
        {
            if (expression is LambdaExpression lambdaExpression)
                return model.GetKey(lambdaExpression.Body);

            if (expression.Strip() is MemberExpression memberExpression)
            {
                return memberExpression.TryGetWellKnownMember() == WellKnownMember.PropertyValue && memberExpression.Expression is MemberExpression sourceMemberExpression
                    ? model.GetKey(sourceMemberExpression.Member)
                    : model.GetKey(memberExpression.Member);
            }

            throw new ExpressionNotSupportedException(expression);
        }

        internal static object GetKey(this IGraphElementPropertyModel model, MemberInfo member)
        {
            return IdentifierDict
                .GetValue(
                    model,
                    closureModel => new KeyLookup(closureModel))
                .GetKey(member);
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return Empty
                .ConfigureMemberMetadata(_ => _
                    .AddRange(models
                        .SelectMany(model => model.Metadata.Keys)
                        .SelectMany(x => x.GetTypeHierarchy())
                        .Distinct()
                        .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Select(property => new KeyValuePair<MemberInfo, MemberMetadata>(property, new MemberMetadata(property.Name)))));
        }
    }
}
