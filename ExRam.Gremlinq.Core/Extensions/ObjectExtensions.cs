using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace LanguageExt
{
    internal static class ObjectExtensions
    {
        private static readonly ConditionalWeakTable<IImmutableDictionary<MemberInfo, PropertyMetadata>, ConcurrentDictionary<Type, (PropertyInfo propertyInfo, object identifier, SerializationBehaviour serializationBehaviour)[]>> TypeProperties = new ConditionalWeakTable<IImmutableDictionary<MemberInfo, PropertyMetadata>, ConcurrentDictionary<Type, (PropertyInfo, object, SerializationBehaviour)[]>>();

        public static IEnumerable<(PropertyInfo property, object identifier, object value)> Serialize(this object obj)
        {
            return Serialize(obj, GraphElementPropertyModel.Default, SerializationBehaviour.Default);
        }

        public static IEnumerable<(PropertyInfo property, object identifier, object value)> Serialize(this object obj, IGraphElementPropertyModel model, SerializationBehaviour ignoreMask)
        {
            var propertyInfoTuples = TypeProperties
                .GetOrCreateValue(model.Metadata)
                .GetOrAdd(
                    obj.GetType(),
                    type => type
                        .GetTypeHierarchy()
                        .SelectMany(typeInHierarchy => typeInHierarchy.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Select(p =>
                        {
                            var metadata = model.Metadata
                                .GetValueOrDefault(p, new PropertyMetadata(p.Name));

                            return (
                                property: p,
                                identifier: model.GetIdentifier(metadata),
                                serializationBehaviour: metadata.SerializationBehaviour);
                        })
                        .OrderBy(x => x.property.Name)
                        .ToArray());

            foreach (var (propertyInfo, identifier, serializationBehaviour) in propertyInfoTuples)
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (identifier is T t)
                {
                    actualSerializationBehaviour = SerializationBehaviour.IgnoreOnUpdate;
                    if (t == T.Label)
                        actualSerializationBehaviour = SerializationBehaviour.IgnoreAlways;
                }

                if ((actualSerializationBehaviour & ignoreMask) == 0)
                {
                    var value = propertyInfo.GetValue(obj);

                    if (value != null)
                        yield return (propertyInfo, identifier, value);
                }
            }
        }

        public static object GetId(this object element)
        {
            var pi = element.GetType().GetProperties().FirstOrDefault(p => string.Equals(p.Name, "id", StringComparison.OrdinalIgnoreCase));

            if (pi == null)
            {
                throw new InvalidOperationException($"Unable to determine Id for {element}");
            }

            return pi.GetValue(element);
        }
    }
}
