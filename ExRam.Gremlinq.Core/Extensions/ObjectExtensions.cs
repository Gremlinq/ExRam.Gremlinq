using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ObjectExtensions
    {
        private static readonly ConditionalWeakTable<IImmutableDictionary<MemberInfo, PropertyMetadata>, ConcurrentDictionary<Type, (PropertyInfo propertyInfo, object identifier, SerializationBehaviour serializationBehaviour)[]>> TypeProperties = new ConditionalWeakTable<IImmutableDictionary<MemberInfo, PropertyMetadata>, ConcurrentDictionary<Type, (PropertyInfo, object, SerializationBehaviour)[]>>();

        public static IEnumerable<(PropertyInfo property, object identifier, object value)> Serialize(this object? obj, IGraphElementPropertyModel model, SerializationBehaviour ignoreMask)
        {
            if (obj == null)
                yield break;

            foreach (var (propertyInfo, identifier, serializationBehaviour) in GetSerializationData(model, obj.GetType()))
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (identifier is T t)
                {
                    actualSerializationBehaviour |= SerializationBehaviour.IgnoreOnUpdate;

                    if (T.Label.Equals(t))
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

        public static object GetId(this object element, IGraphElementPropertyModel model)
        {
            var (propertyInfo, _, _) = GetSerializationData(model, element.GetType())
                .FirstOrDefault(info => T.Id.Equals(info.identifier));

            return propertyInfo == null
                ? throw new InvalidOperationException($"Unable to determine Id for {element}")
                : propertyInfo.GetValue(element);
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static (PropertyInfo propertyInfo, object identifier, SerializationBehaviour serializationBehaviour)[] GetSerializationData(IGraphElementPropertyModel model, Type type)
        {
            return TypeProperties
                .GetOrCreateValue(model.Metadata)
                .GetOrAdd(
                    type,
                    (closureType, closureModel) => closureType
                        .GetTypeHierarchy()
                        .SelectMany(typeInHierarchy => typeInHierarchy
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Where(p => p.GetMethod.GetBaseDefinition() == p.GetMethod)
                        .Select(p =>
                        {
                            var metadata = closureModel.Metadata
                                .GetValueOrDefault(p, new PropertyMetadata(p.Name));

                            return (
                                property: p,
                                identifier: closureModel.GetIdentifier(metadata),
                                serializationBehaviour: metadata.SerializationBehaviour);
                        })
                        .OrderBy(x => x.property.Name)
                        .ToArray(),
                    model);
        }
    }
}
