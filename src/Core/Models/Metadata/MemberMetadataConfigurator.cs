using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    internal sealed class MemberMetadataConfigurator<TElement> : IMemberMetadataConfigurator<TElement>
    {
        private readonly Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> _transformation;

        public MemberMetadataConfigurator() : this(_ => _)
        {
        }

        private MemberMetadataConfigurator(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
        {
            _transformation = transformation;
        }

        public IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                static behaviour => behaviour | SerializationBehaviour.IgnoreOnAdd);
        }

        public IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                static behaviour => behaviour | SerializationBehaviour.IgnoreOnUpdate);
        }

        public IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                static behaviour => behaviour | SerializationBehaviour.IgnoreAlways);
        }

        public IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                static _ => SerializationBehaviour.Default);
        }

        public IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name)
        {
            return Configure(
                propertyExpression,
                metaData => new MemberMetadata(name, metaData.SerializationBehaviour));
        }

        public IMemberMetadataConfigurator<TElement> SetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<SerializationBehaviour, SerializationBehaviour> transformation)
        {
            return Configure(
                propertyExpression,
                metaData => new MemberMetadata(metaData.Key, transformation(metaData.SerializationBehaviour)));
        }

        private IMemberMetadataConfigurator<TElement> Configure<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<MemberMetadata, MemberMetadata> transformation)
        {
            var memberInfo = propertyExpression.Body.StripConvert() is MemberExpression memberExpression
                ? memberExpression.Member
                : throw new ExpressionNotSupportedException(propertyExpression);

            return new MemberMetadataConfigurator<TElement>(metadataMap => _transformation(metadataMap).SetItem(
                memberInfo,
                transformation(metadataMap.TryGetValue(memberInfo, out var metadata)
                    ? metadata
                    : new MemberMetadata(memberInfo.Name))));
        }

        public IImmutableDictionary<MemberInfo, MemberMetadata> Transform(IImmutableDictionary<MemberInfo, MemberMetadata> metadata) => _transformation(metadata);
    }
}
