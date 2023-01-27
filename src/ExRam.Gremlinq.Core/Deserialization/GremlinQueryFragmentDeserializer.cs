using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private static readonly MethodInfo CreateFuncMethod1 = typeof(GremlinQueryFragmentDeserializerImpl).GetMethod(nameof(CreateFunc1), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod2 = typeof(GremlinQueryFragmentDeserializerImpl).GetMethod(nameof(CreateFunc2), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod3 = typeof(GremlinQueryFragmentDeserializerImpl).GetMethod(nameof(CreateFunc3), BindingFlags.NonPublic | BindingFlags.Static)!;

            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate> _unconvertedDelegates = new();

            public GremlinQueryFragmentDeserializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object? TryDeserialize<TSerialized>(TSerialized serializedData, Type fragmentType, IGremlinQueryEnvironment environment)
            {
                if (GetUnconvertedDeserializer(typeof(TSerialized), serializedData!.GetType()) is BaseGremlinQueryFragmentDeserializerDelegate<TSerialized> del)
                {
                    if (del(serializedData, fragmentType, environment, this) is { } ret)
                    {
                        if (fragmentType.IsInstanceOfType(ret))
                            return ret;

                        throw new InvalidCastException($"Cannot convert {ret.GetType()} to {fragmentType}.");
                    }

                    return default;
                }

                throw new ArgumentException($"Could not find a deserializer for {fragmentType.FullName}.");
            }

            public IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(
                    _dict.SetItem(
                        typeof(TSerialized),
                        GetUnconvertedDeserializer(typeof(TSerialized), typeof(TSerialized)) is BaseGremlinQueryFragmentDeserializerDelegate<TSerialized> existingFragmentDeserializer
                            ? (fragment, type, env, _, recurse) => deserializer(fragment, type, env, existingFragmentDeserializer, recurse)
                            : deserializer));
            }

            private Delegate GetUnconvertedDeserializer(Type staticType, Type actualType)
            {
                return _unconvertedDelegates
                    .GetOrAdd(
                        (staticType, actualType),
                        static (typeTuple, @this) =>
                        {
                            var (staticType, actualType) = typeTuple;
                        
                            if (@this.InnerLookup(actualType) is { } del)
                            {
                                var effectiveType = del
                                    .GetType()
                                    .GetGenericArguments()[0];

                                if (effectiveType != actualType)
                                {
                                    return (Delegate)CreateFuncMethod1
                                        .MakeGenericMethod(staticType, effectiveType)
                                        .Invoke(null, new object[] { del })!;
                                }

                                return (Delegate)CreateFuncMethod2
                                   .MakeGenericMethod(staticType, effectiveType)
                                   .Invoke(null, new object[] { del })!;
                            }

                            return (Delegate)CreateFuncMethod3
                                .MakeGenericMethod(staticType)
                                .Invoke(null, Array.Empty<object>())!;
                        },
                        this);
            }

            private static BaseGremlinQueryFragmentDeserializerDelegate<TStatic> CreateFunc1<TStatic, TEffective>(GremlinQueryFragmentDeserializerDelegate<TEffective> del)
                where TStatic : TEffective
            {
                return (serialized, fragmentType, environment, recurse) => del(serialized!, fragmentType, environment, static (serialized, _, _, _) => serialized, recurse);
            }

            private static BaseGremlinQueryFragmentDeserializerDelegate<TStatic> CreateFunc2<TStatic, TEffective>(GremlinQueryFragmentDeserializerDelegate<TEffective> del)
                where TEffective : TStatic
            {
                return (serialized, fragmentType, environment, recurse) => del((TEffective)serialized!, fragmentType, environment, static (serialized, _, _, _) => serialized, recurse);
            }

            private static BaseGremlinQueryFragmentDeserializerDelegate<TStatic> CreateFunc3<TStatic>()
            {
                return static (serialized, _, _, _) => serialized;
            }

            private Delegate? InnerLookup(Type actualType)
            {
                if (_dict.TryGetValue(actualType, out var ret))
                    return ret;

                var baseType = actualType.BaseType;

                foreach (var implementedInterface in actualType.GetInterfaces())
                {
                    if ((baseType == null || !implementedInterface.IsAssignableFrom(baseType)) && InnerLookup(implementedInterface) is { } interfaceSerializer)
                        return interfaceSerializer;
                }

                return baseType != null && InnerLookup(baseType) is { } baseSerializer
                    ? baseSerializer
                    : null;
            }
        }

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableDictionary<Type, Delegate>.Empty);

        public static IGremlinQueryFragmentDeserializer AddToStringFallback(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override<object>(static (data, type, env, overridden, recurse) => type == typeof(string)
                ? data.ToString()
                : overridden(data, type, env, recurse));

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TNative>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializerDelegate)
        {
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, overridden, recurse) => type == typeof(TNative)
                    ? deserializerDelegate(token, type, env, overridden, recurse)
                    : overridden(token, type, env, recurse));
        }

        internal static IGremlinQueryFragmentDeserializer ToGraphsonString(this IGremlinQueryFragmentDeserializer deserializer)
        {
            return deserializer
                .Override<object>(static (data, type, env, overridden, recurse) => type.IsAssignableFrom(typeof(string))
                    ? new GraphSON2Writer().WriteObject(data)
                    : overridden(data, type, env, recurse));
        }
    }
}
