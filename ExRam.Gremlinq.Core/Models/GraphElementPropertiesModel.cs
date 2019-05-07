using System;
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertiesModel
    {
        private sealed class GraphElementPropertiesModelImpl : IGraphElementPropertyModel
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

        private sealed class DefaultGraphElementPropertiesModel : IGraphElementPropertyModel
        {
            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => ImmutableDictionary<MemberInfo, MemberMetadata>.Empty;

            public IGraphElementPropertyIdentifierMapping IdentifierMapping => GraphElementPropertyIdentifierMapping.Default;
        }

        private sealed class InvalidGraphElementPropertiesModel : IGraphElementPropertyModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(MetaData)));

            public IGraphElementPropertyIdentifierMapping IdentifierMapping => GraphElementPropertyIdentifierMapping.Invalid;
        }

        public static readonly IGraphElementPropertyModel Default = new DefaultGraphElementPropertiesModel();
        public static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertiesModel();

        public static IGraphElementPropertyModel WithProperties(this IGraphElementPropertyModel model, Func<IGraphElementPropertyIdentifierMapping, IGraphElementPropertyIdentifierMapping> transformation)
        {
            return new GraphElementPropertiesModelImpl(
                transformation(model.IdentifierMapping),
                model.MetaData);
        }

        public static IGraphElementPropertyModel WithCamelCaseProperties(this IGraphElementPropertyModel model)
        {
            return model.WithProperties(_ => _.ToCamelCase());
        }

        public static IGraphElementPropertyModel WithLowerCaseProperties(this IGraphElementPropertyModel model)
        {
            return model.WithProperties(_ => _.ToLowerCase());
        }

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Action<IElementConfigurator<TElement>> action)
        {
            var builder = new ElementConfigurator<TElement>(model.MetaData);

            action(builder);

            return new GraphElementPropertiesModelImpl(model.IdentifierMapping, builder.MetaData);
        }
    }
}
