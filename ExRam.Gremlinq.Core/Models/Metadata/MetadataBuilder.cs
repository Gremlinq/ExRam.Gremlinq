using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal class MetadataBuilder<TElement> : IMetadataBuilder<TElement>
    {
        private IMetadataStore _metadataStore;

        public MetadataBuilder(IMetadataStore metadataStore)
        {
            _metadataStore = metadataStore;
        }

        public IMetadataBuilder<TElement> ReadOnly<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateMetadata(typeof(TElement), propertyExpression, (metadata) => metadata.IsReadOnly = true);

            return this;
        }

        public IMetadataBuilder<TElement> Ignored<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateMetadata(typeof(TElement), propertyExpression, (metadata) => metadata.IsIgnored = true);

            return this;
        }

        private void UpdateMetadata<TProperty>(Type type, Expression<Func<TElement, TProperty>> propertyExpression, Action<PropertyMetadata> action)
        {
            var pi = propertyExpression.GetPropertyAccess();

            _metadataStore.UpdatePropertyMetadata(type, pi, action);
        }
    }
}
