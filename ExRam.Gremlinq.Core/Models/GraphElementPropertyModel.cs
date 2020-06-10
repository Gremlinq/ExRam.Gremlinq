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
            public GraphElementPropertyModelImpl(IImmutableDictionary<MemberInfo, MemberMetadata> metadata)
            {
                MemberMetadata = metadata;
            }
            
            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    transformation(MemberMetadata));
            }

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
            private readonly ConcurrentDictionary<MemberInfo, Key> _members = new ConcurrentDictionary<MemberInfo, Key>();

            public KeyLookup(IGraphElementPropertyModel model)
            {
                _model = model;
                _configuredTs = new HashSet<T>(model.MemberMetadata
                    .Where(kvp => kvp.Value.Key.RawKey is T)
                    .ToDictionary(kvp => (T)kvp.Value.Key.RawKey, kvp => kvp.Key)
                    .Keys);
            }

            public Key GetKey(MemberInfo member)
            {
                return _members.GetOrAdd(
                    member,
                    (closureMember, closureModel) =>
                    {
                        if (DefaultTs.TryGetValue(closureMember.Name, out var defaultT) && !_configuredTs.Contains(defaultT))
                            return defaultT;

                        return closureModel.MemberMetadata.TryGetValue(closureMember, out var metadata)
                            ? metadata.Key
                            : closureMember.Name;
                    },
                    _model);
            }
        }

        public static readonly IGraphElementPropertyModel Empty = new GraphElementPropertyModelImpl(
            ImmutableDictionary<MemberInfo, MemberMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance));

        private static readonly ConditionalWeakTable<IGraphElementPropertyModel, KeyLookup> KeyLookups = new ConditionalWeakTable<IGraphElementPropertyModel, KeyLookup>();

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IMemberMetadataConfigurator<TElement>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            where TElement : class
        {
            return model.ConfigureMemberMetadata(
                metadata => transformation(new MemberMetadataConfigurator<TElement>(metadata)));
        }

        internal static Key GetKey(this IGraphElementPropertyModel model, Expression expression)
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

        internal static Key GetKey(this IGraphElementPropertyModel model, MemberInfo member)
        {
            return KeyLookups
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
