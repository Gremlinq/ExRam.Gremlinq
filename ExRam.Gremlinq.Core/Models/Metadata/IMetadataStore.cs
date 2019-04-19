using System;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IMetadataStore : ICloneable
    {
        PropertyMetadata TryGetPropertyMetadata(Type elementType, PropertyInfo property);

        void UpdatePropertyMetadata(Type type, PropertyInfo propertyInfo, Action<PropertyMetadata> updateAction);
    }
}
