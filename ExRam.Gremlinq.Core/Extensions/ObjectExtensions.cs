using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<Type, Func<object, IGremlinQueryEnvironment, SerializationBehaviour, IEnumerable<(Key key, object value)>>> _serializerDict = new ConcurrentDictionary<Type, Func<object, IGremlinQueryEnvironment, SerializationBehaviour, IEnumerable<(Key key, object value)>>>();

        public static IEnumerable<(Key key, object value)> Serialize(
            this object? obj,
            IGremlinQueryEnvironment environment,
            SerializationBehaviour ignoreMask)
        {
            if (obj == null)
                return Array.Empty<(Key key, object value)>();

            var func = _serializerDict
                .GetOrAdd(
                    obj.GetType(),
                    closureType =>
                    {
                        var interfaces = closureType.GetInterfaces();

                        foreach (var iface in interfaces)
                        {
                            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                            {
                                var method = typeof(ObjectExtensions)
                                    .GetMethod(nameof(SerializeDictionary), BindingFlags.Static | BindingFlags.NonPublic)
                                    .MakeGenericMethod(iface.GetGenericArguments()[0], iface.GetGenericArguments()[1]);

                                return (closureObj, env, behaviour) => (IEnumerable<(Key key, object value)>)method.Invoke(null, new[] { closureObj });
                            }
                        }

                        return SerializeObject;
                    });

            return func(obj, environment, ignoreMask);
        }

        public static object GetId(this object element, IGremlinQueryEnvironment environment)
        {
            var (propertyInfo, _, _) = environment.GetCache().GetSerializationData(element.GetType())
                .FirstOrDefault(info => info.key.RawKey is T t && T.Id.Equals(t));

            return propertyInfo == null
                ? throw new InvalidOperationException($"Unable to determine Id for {element}")
                : propertyInfo.GetValue(element);
        }

        private static IEnumerable<(Key key, object value)> SerializeDictionary<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                if (kvp.Key is {} key && kvp.Value is { } value)
                    yield return (key.ToString(), (object)value);
            }
        }

        private static IEnumerable<(Key key, object value)> SerializeObject(object obj, IGremlinQueryEnvironment environment, SerializationBehaviour ignoreMask)
        {
            foreach (var (propertyInfo, key, serializationBehaviour) in environment.GetCache().GetSerializationData(obj.GetType()))
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (key.RawKey is T t)
                {
                    actualSerializationBehaviour |= environment.Options
                        .GetValue(GremlinqOption.TSerializationBehaviourOverrides)
                        .GetValueOrDefault(t, SerializationBehaviour.Default);
                }

                if ((actualSerializationBehaviour & ignoreMask) == 0)
                {
                    var value = propertyInfo.GetValue(obj);

                    if (value != null)
                        yield return (key, value);
                }
            }
        }
    }
}
