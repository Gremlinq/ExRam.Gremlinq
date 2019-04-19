using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExRam.Gremlinq.Core
{ 
    public class MetadataStore : IMetadataStore
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyMetadata>> _propertyMetadataDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyMetadata>>();

        public void UpdatePropertyMetadata(Type type, PropertyInfo propertyInfo, Action<PropertyMetadata> updateAction)
        {
            var typeDictionary = _propertyMetadataDictionary.GetOrAdd(type, new ConcurrentDictionary<string, PropertyMetadata>());

            var metadata = typeDictionary.GetOrAdd(propertyInfo.Name, new PropertyMetadata());

            updateAction(metadata);
        }

        public PropertyMetadata TryGetPropertyMetadata(Type elementType, PropertyInfo property)
        {
            PropertyMetadata retVal = default;

            if (_propertyMetadataDictionary.TryGetValue(elementType, out var typeDictionary))
            {
                if (typeDictionary.TryGetValue(property.Name, out var metadata))
                {
                    // Treat as immutable
                    retVal = new PropertyMetadata(metadata.IsReadOnly, metadata.IsIgnored);
                }
            }

            return retVal ?? new PropertyMetadata();
        }
    }
}
