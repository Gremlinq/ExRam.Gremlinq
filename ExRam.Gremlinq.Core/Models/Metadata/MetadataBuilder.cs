using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal class MetadataBuilder<TElement> : IMetadataBuilder<TElement>
    {
        private ElementBuilder _parentBuilder;

        public MetadataBuilder(ElementBuilder parentBuilder)
        {
            _parentBuilder = parentBuilder;
        }

        public IMetadataBuilder<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateMetadata(typeof(TElement), propertyExpression, (metadata) => metadata.IsIgnoredOnUpdate = true);

            return this;
        }

        public IMetadataBuilder<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateMetadata(typeof(TElement), propertyExpression, (metadata) => metadata.IsIgnoredAlways = true);

            return this;
        }

        private void UpdateMetadata<TProperty>(Type type, Expression<Func<TElement, TProperty>> propertyExpression, Action<PropertyMetadata> action)
        {
            var pi = propertyExpression.GetPropertyAccess();

            _parentBuilder.UpdatePropertyMetadata(type, pi, action);
        }
    }
}
