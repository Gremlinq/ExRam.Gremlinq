using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public static class QueryFragmentDeserializer
    {
        private sealed class FragmentDeserializerImpl : IQueryFragmentDeserializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private ConcurrentDictionary<Type, Delegate?>? _fastDict;

            public FragmentDeserializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object? TryDeserialize(object serializedData, Type fragmentType, IGremlinQueryEnvironment environment)
            {
                return TryGetDeserializer(serializedData.GetType()) is Func<object, Type, IGremlinQueryEnvironment, Func<object, object?>, IQueryFragmentDeserializer, object?> del
                    ? del(serializedData, fragmentType, environment, _ => throw new NotSupportedException(), this)
                    : serializedData;
            }

            public IQueryFragmentDeserializer Override<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, Func<TSerialized, object?>, IQueryFragmentDeserializer, object?> deserializer)
            {
                return new FragmentDeserializerImpl(
                    _dict.SetItem(
                        typeof(TSerialized),
                        InnerLookup(typeof(TSerialized)) is Func<object, Type, IGremlinQueryEnvironment, Func<object, object?>, IQueryFragmentDeserializer, object?> existingAtomSerializer
                            ? new Func<object, Type, IGremlinQueryEnvironment, Func<object, object?>, IQueryFragmentDeserializer, object?>((atom, type, env, baseSerializer, recurse) => deserializer((TSerialized)atom, type, env, _ => existingAtomSerializer(_!, type, env, baseSerializer, recurse), recurse))
                            : (atom, type, env, baseSerializer, recurse) => deserializer((TSerialized)atom, type, env, _ => baseSerializer(_!), recurse)));
            }

            private Delegate? TryGetDeserializer(Type staticType)
            {
                var fastDict = Volatile.Read(ref _fastDict);
                if (fastDict == null)
                    Volatile.Write(ref _fastDict, fastDict = new ConcurrentDictionary<Type, Delegate?>());

                return fastDict
                    .GetOrAdd(
                        staticType,
                        (closureType, @this) => @this.InnerLookup(closureType),
                        this);
            }

            private Delegate? InnerLookup(Type actualType)
            {
                if (_dict.TryGetValue(actualType, out var ret))
                    return ret;

                foreach (var implementedInterface in actualType.GetInterfaces())
                {
                    if (InnerLookup(implementedInterface) is { } interfaceSerializer)
                        return interfaceSerializer;
                }

                if (actualType.BaseType is { } baseType)
                {
                    if (InnerLookup(baseType) is { } baseSerializer)
                        return baseSerializer;
                }

                return null;
            }
        }

        public static readonly IQueryFragmentDeserializer Identity = new FragmentDeserializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
    }
}
