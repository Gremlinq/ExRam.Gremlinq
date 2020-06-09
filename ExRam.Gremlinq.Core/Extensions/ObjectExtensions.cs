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
        private static readonly ConditionalWeakTable<IImmutableDictionary<MemberInfo, MemberMetadata>, ConcurrentDictionary<Type, (PropertyInfo propertyInfo, object identifier, SerializationBehaviour serializationBehaviour)[]>> TypeProperties = new ConditionalWeakTable<IImmutableDictionary<MemberInfo, MemberMetadata>, ConcurrentDictionary<Type, (PropertyInfo, object, SerializationBehaviour)[]>>();

        public static IEnumerable<(object identifier, object value)> Serialize(
            this object? obj,
            IGremlinQueryEnvironment environment,
            SerializationBehaviour ignoreMask)
        {
            if (obj == null)
                yield break;

            foreach (var (propertyInfo, identifier, serializationBehaviour) in GetSerializationData(environment.Model.PropertiesModel, obj.GetType()))
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (identifier is T t)
                {
                    actualSerializationBehaviour |= environment.Options
                        .GetValue(GremlinqOption.TSerializationBehaviourOverrides)
                        .GetValueOrDefault(t, SerializationBehaviour.Default);
                }

                if ((actualSerializationBehaviour & ignoreMask) == 0)
                {
                    var value = propertyInfo.GetValue(obj);

                    if (value != null)
                        yield return (identifier, value);
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
                .GetOrCreateValue(model.MemberMetadata)
                .GetOrAdd(
                    type,
                    (closureType, closureModel) => closureType
                        .GetTypeHierarchy()
                        .SelectMany(typeInHierarchy => typeInHierarchy
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Where(p => p.GetMethod.GetBaseDefinition() == p.GetMethod)
                        .Select(p =>
                        {
                            return (
                                property: p,
                                identifier: closureModel.GetKey(p),
                                serializationBehaviour: closureModel.MemberMetadata
                                    .GetValueOrDefault(p, new MemberMetadata(p.Name)).SerializationBehaviour);
                        })
                        .OrderBy(x => x.property.Name)
                        .ToArray(),
                    model);
        }
    }
}
