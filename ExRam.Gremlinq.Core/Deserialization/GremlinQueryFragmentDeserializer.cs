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

            public IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(
                    _dict.SetItem(
                        typeof(TSerialized),
                        TryGetDeserializer(typeof(TSerialized), typeof(TSerialized)) is Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> existingFragmentDeserializer
                            ? (fragment, type, env, baseSerializer, recurse) => deserializer(fragment, type, env, existingFragmentDeserializer, recurse)
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
                                //return (TStatic serialized, Type fragmentType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse) => del((TEffective)serialized, fragmentType, environment, (TEffective _, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer) => _, recurse);

                                var effectiveType = del.GetType().GetGenericArguments()[0];

                                var serializedParameter = Expression.Parameter(staticType);
                                var fragmentTypeParameter = Expression.Parameter(typeof(Type));
                                var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var recurseParameter = Expression.Parameter(typeof(IGremlinQueryFragmentDeserializer));

                                var argument4Parameter1 = Expression.Parameter(effectiveType);
                                var argument4Parameter2 = Expression.Parameter(typeof(Type));
                                var argument4Parameter3 = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var argument4Parameter4 = Expression.Parameter(typeof(IGremlinQueryFragmentDeserializer));

                                var staticFuncType = typeof(Func<,,,,>).MakeGenericType(
                                    staticType,
                                    fragmentTypeParameter.Type,
                                    environmentParameter.Type,
                                    recurseParameter.Type,
                                    typeof(object));

                                var overrideFuncType = typeof(Func<,,,,>).MakeGenericType(
                                    argument4Parameter1.Type,
                                    argument4Parameter2.Type,
                                    argument4Parameter3.Type,
                                    argument4Parameter4.Type,
                                    typeof(object));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    Expression.Convert(
                                        serializedParameter,
                                        effectiveType),
                                    fragmentTypeParameter,
                                    environmentParameter,
                                    Expression.Lambda(
                                        overrideFuncType,
                                        Expression.Convert(argument4Parameter1, typeof(object)),
                                        argument4Parameter1,
                                        argument4Parameter2,
                                        argument4Parameter3,
                                        argument4Parameter4),
                                    recurseParameter);

                                return Expression
                                    .Lambda(
                                        staticFuncType,
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
    }
}
