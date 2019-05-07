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
            public GraphElementPropertyModelImpl(
                IImmutableDictionary<MemberInfo, PropertyMetadata> metaData)
            {
                MetaData = metaData;
            }

            public IImmutableDictionary<MemberInfo, PropertyMetadata> MetaData { get; }
        }

        private sealed class DefaultGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public IImmutableDictionary<MemberInfo, PropertyMetadata> MetaData => ImmutableDictionary<MemberInfo, PropertyMetadata>.Empty;
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, PropertyMetadata> MetaData => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(MetaData)));
        }

        public static readonly IGraphElementPropertyModel Default = new DefaultGraphElementPropertyModel();
        public static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();

        public static IGraphElementPropertyModel WithCamelCaseProperties(this IGraphElementPropertyModel model)
        {
            return model.WithMetadata(_ => _.ToCamelCase());
        }

        public static IGraphElementPropertyModel WithLowerCaseProperties(this IGraphElementPropertyModel model)
        {
            return model.WithMetadata(_ => _.ToLowerCase());
        }

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Action<IElementConfigurator<TElement>> action)
        {
            var builder = new ElementConfigurator<TElement>(model.MetaData);

            action(builder);

            return new GraphElementPropertyModelImpl(builder.MetaData);
        }

        internal static object GetIdentifier(this IGraphElementPropertyModel model, MemberInfo member)
        {
            var identifier = model.MetaData
                .TryGetValue(member)
                .Bind(x => x.IdentifierOverride)
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
                    .SelectMany(model => model.Labels.Keys)
                    .SelectMany(x => x.GetTypeHierarchy())
                    .Distinct()
                    .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    .ToImmutableDictionary(
                        property => (MemberInfo)property,
                        property => PropertyMetadata.Default));
        }

        internal static IGraphElementPropertyModel WithMetadata(this IGraphElementPropertyModel model, Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation)
        {
            return new GraphElementPropertyModelImpl(
                transformation(model.MetaData));
        }
    }
}
