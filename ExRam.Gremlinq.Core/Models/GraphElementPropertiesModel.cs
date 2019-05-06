using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.Extensions;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertiesModel
    {
        private sealed class GraphElementPropertiesModelImpl : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _baseModel;

            public GraphElementPropertiesModelImpl(
                IGraphElementPropertiesModel baseModel,
                IImmutableDictionary<MemberInfo, MemberMetadata> metaData)
            {
                _baseModel = baseModel;
                MetaData = metaData;
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData { get; }

            public object GetIdentifier(Expression expression) => _baseModel.GetIdentifier(expression);
        }

        private sealed class DefaultGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => ImmutableDictionary<MemberInfo, MemberMetadata>.Empty;

            public object GetIdentifier(Expression expression)
            {
                if (expression is MemberExpression memberExpression)
                {
                    var memberName = memberExpression.Member.Name;

                    if (string.Equals(memberName, "id", StringComparison.OrdinalIgnoreCase))
                        return T.Id;

                    if (string.Equals(memberName, "label", StringComparison.OrdinalIgnoreCase))
                        return T.Label;

                    return memberName;
                }

                throw new ExpressionNotSupportedException(expression);
            }
        }

        private sealed class InvalidGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(MetaData)));

            public object GetIdentifier(Expression expression) => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(GetIdentifier)));
        }

        private sealed class CamelCaseGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _model;

            public CamelCaseGraphElementPropertiesModel(IGraphElementPropertiesModel model)
            {
                _model = model;
            }

            public object GetIdentifier(Expression expression)
            {
                var retVal = _model.GetIdentifier(expression);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => _model.MetaData;
        }

        private sealed class LowerCaseGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _model;

            public LowerCaseGraphElementPropertiesModel(IGraphElementPropertiesModel model)
            {
                _model = model;
            }

            public object GetIdentifier(Expression expression)
            {
                var retVal = _model.GetIdentifier(expression);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData => _model.MetaData;
        }

        public static readonly IGraphElementPropertiesModel Default = new DefaultGraphElementPropertiesModel();
        public static readonly IGraphElementPropertiesModel Invalid = new InvalidGraphElementPropertiesModel();

        public static IGraphElementPropertiesModel WithCamelCaseProperties(this IGraphElementPropertiesModel model)
        {
            return new CamelCaseGraphElementPropertiesModel(model);
        }

        public static IGraphElementPropertiesModel WithLowerCaseProperties(this IGraphElementPropertiesModel model)
        {
            return new LowerCaseGraphElementPropertiesModel(model);
        }

        public static IGraphElementPropertiesModel ConfigureElement<TElement>(this IGraphElementPropertiesModel model, Action<IElementConfigurator<TElement>> action)
        {
            var builder = new ElementConfigurator<TElement>();

            action(builder);

            var dict = model.MetaData;
            
            foreach (var updateSemanticsKvp in builder.UpdateSemantics)
            {
                dict = dict.SetItem(updateSemanticsKvp.Key, new MemberMetadata(updateSemanticsKvp.Value));
            }

            return new GraphElementPropertiesModelImpl(model, dict);
        }
    }
}