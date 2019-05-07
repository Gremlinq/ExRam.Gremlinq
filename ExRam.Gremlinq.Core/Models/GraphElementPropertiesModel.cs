using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertyModel
    {
        private sealed class GraphElementPropertyModelImpl : IGraphElementPropertyModel
        {
            public GraphElementPropertyModelImpl(IImmutableDictionary<MemberInfo, PropertyMetadata> metadata)
            {
                Metadata = metadata;
            }

            public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata { get; }
        }

        private sealed class DefaultGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata => ImmutableDictionary<MemberInfo, PropertyMetadata>.Empty;
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(Metadata)));
        }

        public static readonly IGraphElementPropertyModel Default = new DefaultGraphElementPropertyModel();
        public static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();

        public static IGraphElementPropertyModel WithCamelCaseNames(this IGraphElementPropertyModel model)
        {
            return model.WithMetadata(_ => _.WithCamelCaseNames());
        }

        public static IGraphElementPropertyModel WithLowerCaseNames(this IGraphElementPropertyModel model)
        {
            return model.WithMetadata(_ => _.WithLowerCaseNames());
        }

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IPropertyMetadataBuilder<TElement>, IImmutableDictionary<MemberInfo, PropertyMetadata>> action)
        {
            return new GraphElementPropertyModelImpl(action(new PropertyMetadataBuilder<TElement>(model.Metadata)));
        }

        internal static object GetIdentifier(this IGraphElementPropertyModel model, MemberInfo member)
        {
            var identifier = model.Metadata
                .TryGetValue(member)
                .Bind(x => x.NameOverride)
                .IfNone(member.Name);
            
            if (string.Equals(identifier, "id", StringComparison.OrdinalIgnoreCase))
                return T.Id;

            if (string.Equals(identifier, "label", StringComparison.OrdinalIgnoreCase))
                return T.Label;

            return identifier;
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return new GraphElementPropertyModelImpl(
                models
                    .SelectMany(model => model.Metadata.Keys)
                    .SelectMany(x => x.GetTypeHierarchy())
                    .Distinct()
                    .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    .ToImmutableDictionary(
                        property => (MemberInfo)property,
                        property => PropertyMetadata.Default));
        }

        private static IGraphElementPropertyModel WithMetadata(this IGraphElementPropertyModel model, Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation)
        {
            return new GraphElementPropertyModelImpl(
                transformation(model.Metadata));
        }
    }
}
