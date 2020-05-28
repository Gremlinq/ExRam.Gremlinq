using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentSerializer
    {
        private sealed class GremlinQueryFragmentSerializerImpl : IGremlinQueryFragmentSerializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>();

            public GremlinQueryFragmentSerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment)
            {
                return TryGetSerializer(typeof(TFragment), fragment!.GetType()) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object> del
                    ? del(fragment, gremlinQueryEnvironment, this)
                    : fragment;
            }

            public IGremlinQueryFragmentSerializer Override<TFragment>(Func<TFragment, IGremlinQueryEnvironment, Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object>, IGremlinQueryFragmentSerializer, object> serializer)
            {
                return new GremlinQueryFragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        TryGetSerializer(typeof(TFragment), typeof(TFragment)) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object> existingFragmentSerializer
                            ? (fragment, env, baseSerializer, recurse) => serializer(fragment, env, existingFragmentSerializer, recurse)
                            : serializer));
            }

            private Delegate? TryGetSerializer(Type staticType, Type actualType)
            {
                return _fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        (typeTuple, @this) =>
                        {
                            var (staticType, actualType) = typeTuple;

                            if (@this.InnerLookup(actualType) is { } del)
                            {
                                //return (TStatic fragment, IGremlinQueryEnvironment environment, IGremlinQueryFragmentSerializer recurse) => del((TEffective)fragment, environment, (TEffective _, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer) => _, recurse);

                                var effectiveType = del.GetType().GetGenericArguments()[0];
                                var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var recurseParameter = Expression.Parameter(typeof(IGremlinQueryFragmentSerializer));

                                var argument3Parameter1 = Expression.Parameter(effectiveType);
                                var argument3Parameter2 = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var argument3Parameter3 = Expression.Parameter(typeof(IGremlinQueryFragmentSerializer));

                                var fragmentParameterExpression = Expression.Parameter(staticType);

                                var staticTypeFunc = typeof(Func<,,,>).MakeGenericType(
                                    staticType,
                                    environmentParameter.Type,
                                    recurseParameter.Type,
                                    typeof(object));

                                var effectiveTypeFunc = typeof(Func<,,,>).MakeGenericType(
                                    argument3Parameter1.Type,
                                    argument3Parameter2.Type,
                                    argument3Parameter3.Type,
                                    typeof(object));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    Expression.Convert(
                                        fragmentParameterExpression,
                                        effectiveType),
                                    environmentParameter,
                                    Expression.Lambda(
                                        effectiveTypeFunc,
                                        Expression.Convert(argument3Parameter1, typeof(object)),
                                        argument3Parameter1,
                                        argument3Parameter2,
                                        argument3Parameter3),
                                    recurseParameter);

                                return Expression
                                    .Lambda(
                                        staticTypeFunc,
                                        retCall,
                                        fragmentParameterExpression,
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

        public static readonly IGremlinQueryFragmentSerializer Identity = new GremlinQueryFragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
    }
}
