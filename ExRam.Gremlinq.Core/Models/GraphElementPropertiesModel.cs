using System;
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertiesModel
    {
        private sealed class GraphElementPropertiesModelImpl : IGraphElementPropertiesModel
        {
            public GraphElementPropertiesModelImpl(
                IGraphElementPropertyIdentifierMapping identifierMapping,
                IImmutableDictionary<MemberInfo, MemberMetadata> metaData)
            {
                IdentifierMapping = identifierMapping;
                MetaData = metaData;
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData { get; }

            public IGraphElementPropertyIdentifierMapping IdentifierMapping { get; }
        }

        private sealed class DefaultGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => ImmutableDictionary<MemberInfo, MemberMetadata>.Empty;

            public IGraphElementPropertyIdentifierMapping IdentifierMapping => GraphElementPropertyIdentifierMapping.Default;
        }

        private sealed class InvalidGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(MetaData)));

            public IGraphElementPropertyIdentifierMapping IdentifierMapping => GraphElementPropertyIdentifierMapping.Invalid;
        }

        public static readonly IGraphElementPropertiesModel Default = new DefaultGraphElementPropertiesModel();
        public static readonly IGraphElementPropertiesModel Invalid = new InvalidGraphElementPropertiesModel();

        public static IGraphElementPropertiesModel WithProperties(this IGraphElementPropertiesModel model, Func<IGraphElementPropertyIdentifierMapping, IGraphElementPropertyIdentifierMapping> transformation)
        {
            return new GraphElementPropertiesModelImpl(
                transformation(model.IdentifierMapping),
                model.MetaData);
        }

        public static IGraphElementPropertiesModel WithCamelCaseProperties(this IGraphElementPropertiesModel model)
        {
            return model.WithProperties(_ => _.ToCamelCase());
        }

        public static IGraphElementPropertiesModel WithLowerCaseProperties(this IGraphElementPropertiesModel model)
        {
            return model.WithProperties(_ => _.ToLowerCase());
        }

        public static IGraphElementPropertiesModel ConfigureElement<TElement>(this IGraphElementPropertiesModel model, Action<IElementConfigurator<TElement>> action)
        {
            var builder = new ElementConfigurator<TElement>(model.MetaData);

            action(builder);

            return new GraphElementPropertiesModelImpl(model.IdentifierMapping, builder.MetaData);
        }
    }
}
