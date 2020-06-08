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
            public GraphElementPropertyModelImpl(IImmutableDictionary<MemberInfo, PropertyMetadata> metadata, IImmutableDictionary<string, T> specialNames)
            {
                Metadata = metadata;
                SpecialNames = specialNames;
            }
            
            public IGraphElementPropertyModel ConfigureMetadata(Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    transformation(Metadata),
                    SpecialNames);
            }

            public IGraphElementPropertyModel ConfigureSpecialNames(Func<IImmutableDictionary<string, T>, IImmutableDictionary<string, T>> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    Metadata,
                    transformation(SpecialNames));
            }

            public IImmutableDictionary<string, T> SpecialNames { get; }

            public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata { get; }
        }

        public static readonly IGraphElementPropertyModel Default = new GraphElementPropertyModelImpl(
            ImmutableDictionary<MemberInfo, PropertyMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance),
            ImmutableDictionary<string, T>
                .Empty
                .WithComparers(StringComparer.OrdinalIgnoreCase)
                .Add("id", T.Id)
                .Add("label", T.Label));

        private static readonly ConditionalWeakTable<IGraphElementPropertyModel, ConcurrentDictionary<MemberInfo, object>> IdentifierDict = new ConditionalWeakTable<IGraphElementPropertyModel, ConcurrentDictionary<MemberInfo, object>>();

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IPropertyMetadataConfigurator<TElement>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation)
            where TElement : class
        {
            return model.ConfigureMetadata(
                metadata => transformation(new PropertyMetadataConfigurator<TElement>(metadata)));
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

        private static object GetKey(this IGraphElementPropertyModel model, MemberInfo member)
        {
            return IdentifierDict
                .GetOrCreateValue(model)
                .GetOrAdd(
                    member,
                    (closureMember, closureModel) => closureModel.GetKey(closureModel.Metadata.TryGetValue(closureMember, out var metadata)
                        ? metadata
                        : new PropertyMetadata(closureMember.Name)),
                    model);
        }

        internal static object GetKey(this IGraphElementPropertyModel model, PropertyMetadata metadata)
        {
            return model.SpecialNames.TryGetValue(metadata.Name, out var name)
                ? (object)name
                : metadata.Name;
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return Default
                .ConfigureMetadata(_ => _
                    .AddRange(models
                        .SelectMany(model => model.Metadata.Keys)
                        .SelectMany(x => x.GetTypeHierarchy())
                        .Distinct()
                        .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Select(property => new KeyValuePair<MemberInfo, PropertyMetadata>(property, new PropertyMetadata(property.Name)))));
        }
    }
}
