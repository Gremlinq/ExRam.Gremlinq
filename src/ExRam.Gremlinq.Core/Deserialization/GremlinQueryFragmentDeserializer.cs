using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private static readonly MethodInfo CreateFuncMethod1 = typeof(GremlinQueryFragmentDeserializerImpl).GetMethod(nameof(CreateFunc1), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod2 = typeof(GremlinQueryFragmentDeserializerImpl).GetMethod(nameof(CreateFunc2), BindingFlags.NonPublic | BindingFlags.Static)!;

            private readonly IImmutableDictionary<Type, GremlinQueryFragmentDeserializerDelegate> _delegates;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDelegates = new();

            public GremlinQueryFragmentDeserializerImpl(IImmutableDictionary<Type, GremlinQueryFragmentDeserializerDelegate> delegates)
            {
                _delegates = delegates;
            }

            public bool TryDeserialize<TSerializedData>(TSerializedData serializedData, Type requestedType, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out object? value)
            {
                if (GetDeserializerDelegate(typeof(TSerializedData), serializedData!.GetType()) is BaseGremlinQueryFragmentDeserializerDelegate<TSerializedData> del)
                {
                    if (del(serializedData, requestedType, environment, this) is { } ret)
                    {
                        if (requestedType.IsInstanceOfType(ret))
                        {
                            value = ret;
                            return true;
                        }

                        throw new InvalidCastException($"Cannot convert {ret.GetType()} to {requestedType}.");
                    }

                    value = null;
                    return false;
                }

                if (requestedType.IsInstanceOfType(serializedData))
                {
                    value = serializedData;
                    return true;
                }

                throw new ArgumentException($"Could not find a deserializer for {requestedType.FullName}.");
            }

            public IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(
                    _delegates.SetItem(
                        typeof(TSerialized),
                        GetDeserializerDelegate(typeof(TSerialized), typeof(TSerialized)) is BaseGremlinQueryFragmentDeserializerDelegate<TSerialized> existingFragmentDeserializer
                            ? GremlinQueryFragmentDeserializerDelegate<TSerialized>.From((fragment, requestedType, env, _, recurse) => deserializer.Execute(fragment, requestedType, env, existingFragmentDeserializer, recurse))
                            : deserializer));
            }

            private Delegate? GetDeserializerDelegate(Type staticSerializedDataType, Type actualSerializedDataType)
            {
                return _fastDelegates
                    .GetOrAdd(
                        (staticSerializedDataType, actualSerializedDataType),
                        static (typeTuple, @this) =>
                        {
                            var (staticSerializedDataType, actualSerializedDataType) = typeTuple;

                            if (@this.InnerLookup(actualSerializedDataType) is { } del)
                            {
                                var effectiveSerializedDataType = del
                                    .GetType()
                                    .GetGenericArguments()[0];

                                var methodInfo = effectiveSerializedDataType != actualSerializedDataType
                                    ? CreateFuncMethod1
                                    : CreateFuncMethod2;

                                return (Delegate?)methodInfo
                                    .MakeGenericMethod(staticSerializedDataType, effectiveSerializedDataType)
                                    .Invoke(null, new object[] { del });
                            }

                            return null;
                        },
                        this);
            }

            private static BaseGremlinQueryFragmentDeserializerDelegate<TStatic> CreateFunc1<TStatic, TEffective>(GremlinQueryFragmentDeserializerDelegate<TEffective> del)
                where TStatic : TEffective
            {
                return (serialized, fragmentType, environment, recurse) => del.Execute(serialized!, fragmentType, environment, static (serialized, _, _, _) => serialized, recurse);
            }

            private static BaseGremlinQueryFragmentDeserializerDelegate<TStatic> CreateFunc2<TStatic, TEffective>(GremlinQueryFragmentDeserializerDelegate<TEffective> del)
                where TEffective : TStatic
            {
                return (serialized, fragmentType, environment, recurse) => del.Execute((TEffective)serialized!, fragmentType, environment, static (serialized, _, _, _) => serialized, recurse);
            }

            private GremlinQueryFragmentDeserializerDelegate? InnerLookup(Type actualType)
            {
                if (_delegates.TryGetValue(actualType, out var ret))
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

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableDictionary<Type, GremlinQueryFragmentDeserializerDelegate>.Empty);

        public static readonly IGremlinQueryFragmentDeserializer Default = Identity
            .Override<object>(static (data, type, env, overridden, recurse) =>
            {
                if (type.IsInstanceOfType(data))
                    return data;

                if (type.IsArray)
                {
                    var elementType = type.GetElementType()!;
                    var ret = Array.CreateInstance(elementType, 1);

                    ret
                        .SetValue(recurse.TryDeserialize(elementType).From(data, env), 0);

                    return ret;
                }

                return overridden(data, type, env, recurse);
            })
            .AddToStringFallback();

        public static IGremlinQueryFragmentDeserializer AddToStringFallback(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override<object>(static (data, type, env, overridden, recurse) => type == typeof(string)
                ? data.ToString()
                : overridden(data, type, env, recurse));

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TNative>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializerDelegate)
        {
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, overridden, recurse) => type == typeof(TNative)
                    ? deserializerDelegate.Execute(token, type, env, overridden, recurse)
                    : overridden(token, type, env, recurse));
        }
    }
}
