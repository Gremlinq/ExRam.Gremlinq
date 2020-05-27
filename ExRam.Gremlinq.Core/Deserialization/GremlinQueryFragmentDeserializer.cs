using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>();

            public GremlinQueryFragmentDeserializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object? TryDeserialize<TSerialized>(TSerialized serializedData, Type fragmentType, IGremlinQueryEnvironment environment)
            {
                return TryGetDeserializer(typeof(TSerialized), serializedData!.GetType()) is Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> del
                    ? del(serializedData, fragmentType, environment, this)
                    : serializedData;
            }

            public IGremlinQueryFragmentDeserializer Override<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, Func<TSerialized, object?>, IGremlinQueryFragmentDeserializer, object?> deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(
                    _dict.SetItem(
                        typeof(TSerialized),
                        TryGetDeserializer(typeof(TSerialized), typeof(TSerialized)) is Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> existingFragmentDeserializer
                            ? (fragment, type, env, baseSerializer, recurse) => deserializer(fragment, type, env, _ => existingFragmentDeserializer(_, type, env, recurse), recurse)
                            : deserializer));
            }

            private Delegate? TryGetDeserializer(Type staticType, Type actualType)
            {
                return _fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        (typeTuple, @this) =>
                        {
                            var (staticType, actualType) = typeTuple;

                            if (@this.InnerLookup(actualType) is { } del)
                            {
                                //return (TStatic serialized, Type fragmentType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse) => del((TActualType)serialized, fragmentType, environment, (TActual _) => _, recurse);

                                var effectiveType = del.GetType().GetGenericArguments()[0];
                                var argument4Parameter = Expression.Parameter(effectiveType);

                                var serializedParameter = Expression.Parameter(staticType);
                                var fragmentTypeParameter = Expression.Parameter(typeof(Type));
                                var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var recurseParameter = Expression.Parameter(typeof(IGremlinQueryFragmentDeserializer));

                                var effectiveTypeFunc = typeof(Func<,>).MakeGenericType(effectiveType, typeof(object));
                                var staticTypeFunc = typeof(Func<,,,,>).MakeGenericType(
                                    staticType,
                                    fragmentTypeParameter.Type,
                                    environmentParameter.Type,
                                    recurseParameter.Type,
                                    typeof(object));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    Expression.Convert(
                                        serializedParameter,
                                        effectiveType),
                                    fragmentTypeParameter,
                                    environmentParameter,
                                    Expression.Lambda(
                                        effectiveTypeFunc,
                                        Expression.Convert(argument4Parameter, typeof(object)),
                                        argument4Parameter),
                                    recurseParameter);

                                return Expression
                                    .Lambda(
                                        staticTypeFunc,
                                        retCall,
                                        serializedParameter,
                                        fragmentTypeParameter,
                                        environmentParameter,
                                        recurseParameter)
                                    .Compile();
                            }

                            return null;
                        },
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

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
    }
}
