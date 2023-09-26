using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.Models
{
    internal sealed class MemberMetadataConfigurator<TElement> : IMemberMetadataConfigurator<TElement>
    {
        private readonly Func<IGraphElementModel, IGraphElementModel> _transformation;

        public MemberMetadataConfigurator() : this(_ => _)
        {
        }

        private MemberMetadataConfigurator(Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            _transformation = transformation;
        }

        public IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression) => SetSerializationBehaviour(
            propertyExpression,
            static behaviour => behaviour | SerializationBehaviour.IgnoreOnAdd);

        public IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression) => SetSerializationBehaviour(
            propertyExpression,
            static behaviour => behaviour | SerializationBehaviour.IgnoreOnUpdate);

        public IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression) => SetSerializationBehaviour(
            propertyExpression,
            static behaviour => behaviour | SerializationBehaviour.IgnoreAlways);

        public IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression) => SetSerializationBehaviour(
            propertyExpression,
            static _ => SerializationBehaviour.Default);

        public IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name) => Configure(
            propertyExpression,
            metaData => new MemberMetadata(name, metaData.SerializationBehaviour));

        public IMemberMetadataConfigurator<TElement> SetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<SerializationBehaviour, SerializationBehaviour> transformation) => Configure(
            propertyExpression,
            metaData => new MemberMetadata(metaData.Key, transformation(metaData.SerializationBehaviour)));

        public IGraphElementModel Transform(IGraphElementModel model) => _transformation(model);

        private IMemberMetadataConfigurator<TElement> Configure<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<MemberMetadata, MemberMetadata> transformation)
        {
            var memberInfo = propertyExpression.Body.StripConvert() is MemberExpression memberExpression
                ? memberExpression.Member
                : throw new ExpressionNotSupportedException(propertyExpression);

            return new MemberMetadataConfigurator<TElement>(model => _transformation(model).ConfigureMetadata(
                memberInfo,
                metadata => transformation(metadata)));
        }
    }
}
