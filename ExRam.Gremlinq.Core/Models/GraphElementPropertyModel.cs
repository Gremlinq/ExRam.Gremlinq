using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;
using LanguageExt;

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

            public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata { get; }

            public IImmutableDictionary<string, T> SpecialNames { get; }
        }

        public static readonly IGraphElementPropertyModel Default = new GraphElementPropertyModelImpl(
            ImmutableDictionary<MemberInfo, PropertyMetadata>.Empty,
            ImmutableDictionary<string, T>.Empty
                .WithComparers(StringComparer.OrdinalIgnoreCase)
                .Add("id", T.Id)
                .Add("label", T.Label));

        private static readonly ConditionalWeakTable<IGraphElementPropertyModel, ConcurrentDictionary<MemberInfo, object>> IdentifierDict = new ConditionalWeakTable<IGraphElementPropertyModel, ConcurrentDictionary<MemberInfo, object>>();

        public static IGraphElementPropertyModel ConfigureNames(this IGraphElementPropertyModel model, Func<MemberInfo, string, string> overrideTransformation)
        {
            return model.ConfigureMetadata(_ => _.ConfigureNames(overrideTransformation));
        }
        
        public static IGraphElementPropertyModel UseCamelCaseNames(this IGraphElementPropertyModel model)
        {
            return model.ConfigureNames((member, name) => name.ToCamelCase());
        }

        public static IGraphElementPropertyModel WithLowerCaseNames(this IGraphElementPropertyModel model)
        {
            return model.ConfigureNames((member, name) => name.ToLower());
        }

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IPropertyMetadataConfigurator<TElement>, IImmutableDictionary<MemberInfo, PropertyMetadata>> action)
            where TElement : class
        {
            return model.ConfigureMetadata(
                metadata => action(new PropertyMetadataConfigurator<TElement>(metadata)));
        }

        internal static object GetIdentifier(this IGraphElementPropertyModel model, MemberExpression memberExpression)
        {
            if (memberExpression.IsPropertyValue() && memberExpression.Expression is MemberExpression sourceMemberExpression)
                return model.GetIdentifier(sourceMemberExpression);

            return model.GetIdentifier(memberExpression.Member);
        }

        private static object GetIdentifier(this IGraphElementPropertyModel model, MemberInfo member)
        {
            return IdentifierDict
                .GetOrCreateValue(model)
                .GetOrAdd(
                    member,
                    closureMember =>
                    {
                        if (closureMember.DeclaringType != null && closureMember.DeclaringType.IsInterface)
                        {
                            var interfaceGetter = ((PropertyInfo)closureMember).GetMethod;

                            var implementingGetters = model.Metadata.Keys
                                .Select(x => x.DeclaringType)
                                .Distinct()
                                .Where(declaringType => closureMember.DeclaringType.IsAssignableFrom(declaringType))
                                .Select(declaringType =>
                                {
                                    var interfaceMap = declaringType
                                        .GetInterfaceMap(closureMember.DeclaringType);

                                    var index = Array.IndexOf(
                                        interfaceMap.InterfaceMethods,
                                        interfaceGetter);

                                    return interfaceMap.TargetMethods[index];
                                })
                                .ToArray();

                            if (implementingGetters.Length > 0)
                            {
                                var identifiers = model.Metadata.Keys
                                    .Where(m => closureMember.DeclaringType.IsAssignableFrom(m.DeclaringType))
                                    .OfType<PropertyInfo>()
                                    .Where(p => implementingGetters.Contains(p.GetMethod, MemberInfoEqualityComparer.Instance))
                                    .Select(model.GetIdentifier)
                                    .Distinct()
                                    .ToArray();
                                
                                if (identifiers.Length > 1)
                                    throw new InvalidOperationException($"Contradicting identifiers found for member {closureMember}.");

                                if (identifiers.Length == 1)
                                    return identifiers[0];
                            }
                        }

                        return model.GetIdentifier(model.Metadata
                            .TryGetValue(closureMember)
                            .IfNone(new PropertyMetadata(closureMember.Name)));
                    });
        }

        internal static object GetIdentifier(this IGraphElementPropertyModel model, PropertyMetadata metadata)
        {
            return model.SpecialNames
                .TryGetValue(metadata.Name)
                .Map(x => (object)x)
                .IfNone(metadata.Name);
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return Default
                .ConfigureMetadata(_ => models
                    .SelectMany(model => model.Metadata.Keys)
                    .SelectMany(x => x.GetTypeHierarchy())
                    .Distinct()
                    .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    .ToImmutableDictionary(
                        property => property,
                        property => new PropertyMetadata(property.Name),
                        MemberInfoEqualityComparer.Instance));
        }

        private static IGraphElementPropertyModel ConfigureMetadata(this IGraphElementPropertyModel model, Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation)
        {
            return new GraphElementPropertyModelImpl(
                transformation(model.Metadata),
                model.SpecialNames);
        }
    }
}
