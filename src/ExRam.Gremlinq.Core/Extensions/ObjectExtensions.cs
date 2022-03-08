using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExRam.Gremlinq.Core.Models;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<Type, Func<object, IGremlinQueryEnvironment, SerializationBehaviour, IEnumerable<(Key key, object value)>>> SerializerDict = new();

        public static TResult Apply<TSource, TResult>(this TSource source, Func<TSource, TResult> transformation) => transformation(source);

        public static IEnumerable<(Key key, object value)> Serialize(
            this object? obj,
            IGremlinQueryEnvironment environment,
            SerializationBehaviour ignoreMask)
        {
            if (obj == null)
                return Array.Empty<(Key key, object value)>();

            var func = SerializerDict
                .GetOrAdd(
                    obj.GetType(),
                    closureType =>
                    {
                        var interfaces = closureType.GetInterfaces();

                        foreach (var iface in interfaces)
                        {
                            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                            {
                                return (Func<object, IGremlinQueryEnvironment, SerializationBehaviour, IEnumerable<(Key key, object value)>>)typeof(ObjectExtensions)
                                    .GetMethod(nameof(CreateSerializeDictionaryFunc), BindingFlags.Static | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(iface.GetGenericArguments()[0], iface.GetGenericArguments()[1])
                                    .Invoke(null, Array.Empty<object>())!;
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

            // ReSharper disable once ConstantConditionalAccessQualifier
            return propertyInfo?.GetValue(element) is { } value
                ? value
                : throw new InvalidOperationException($"Unable to determine Id for {element}");
        }

        private static Func<object, IGremlinQueryEnvironment, SerializationBehaviour, IEnumerable<(Key key, object value)>> CreateSerializeDictionaryFunc<TKey, TValue>()
        {
            return (dict, _, _) => SerializeDictionary((IDictionary<TKey, TValue>)dict);
        }

        private static IEnumerable<(Key key, object value)> SerializeDictionary<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                if (kvp.Key is {} key && kvp.Value is { } value && key.ToString() is { } stringKey)
                    yield return (stringKey, value);
            }
        }

        private static IEnumerable<(Key key, object value)> SerializeObject(object obj, IGremlinQueryEnvironment environment, SerializationBehaviour ignoreMask)
        {
            var serializationBehaviourOverrides = environment.Options
                .GetValue(GremlinqOption.TSerializationBehaviourOverrides);

            foreach (var (propertyInfo, key, serializationBehaviour) in environment.GetCache().GetSerializationData(obj.GetType()))
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (key.RawKey is T t)
                {
                    actualSerializationBehaviour |= serializationBehaviourOverrides
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
