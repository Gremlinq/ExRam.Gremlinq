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
                IImmutableDictionary<MemberInfo, MemberMetadata> metaData)
            {
                MetaData = metaData;
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData { get; }
        }

        private sealed class DefaultGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => ImmutableDictionary<MemberInfo, MemberMetadata>.Empty;
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(MetaData)));
        }

        public static readonly IGraphElementPropertyModel Default = new DefaultGraphElementPropertyModel();
        public static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();

        public static object GetIdentifier(this IGraphElementPropertyModel model, MemberInfo member)
        {
            var identifier = model.MetaData
                .TryGetValue(member)
                .Map(x => x.Identifier)
                .IfNone(member.Name);
            
            if (string.Equals(identifier, "id", StringComparison.OrdinalIgnoreCase))
                return T.Id;

            if (string.Equals(identifier, "label", StringComparison.OrdinalIgnoreCase))
                return T.Label;

            return identifier;
        }

        public static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return new GraphElementPropertyModelImpl(
                models
                    .SelectMany(model => model.Labels.Keys)
                    .SelectMany(type => type.GetProperties())
                    .ToImmutableDictionary(
                        property => (MemberInfo)property,
                        property => new MemberMetadata(property.Name, SerializationDirective.Default)));
        }

        public static IGraphElementPropertyModel WithMetadata(this IGraphElementPropertyModel model, Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
        {
            return new GraphElementPropertyModelImpl(
                transformation(model.MetaData));
        }

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
    }
}
