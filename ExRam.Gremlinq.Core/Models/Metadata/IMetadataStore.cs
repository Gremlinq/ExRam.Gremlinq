using System;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IMetadataStore
    {
        PropertyMetadata TryGetPropertyMetadata(Type elementType, PropertyInfo property);

        void UpdatePropertyMetadata(Type type, PropertyInfo propertyInfo, Action<PropertyMetadata> updateAction);
    }
}
