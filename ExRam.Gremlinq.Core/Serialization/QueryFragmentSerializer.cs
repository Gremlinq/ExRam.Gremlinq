using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public static class QueryFragmentSerializer
    {
        private sealed class QueryFragmentSerializerImpl : IQueryFragmentSerializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>();

            public QueryFragmentSerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment)
            {
                return TryGetSerializer(typeof(TFragment), fragment.GetType()) is Func<TFragment, object> del
                    ? del(fragment)
                    : fragment;
            }

            public IQueryFragmentSerializer Override<TFragment>(Func<TFragment, Func<TFragment, object>, IQueryFragmentSerializer, object> serializer)
            {
                return new QueryFragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        InnerLookup(typeof(TFragment)) is Func<TFragment, Func<TFragment, object>, IQueryFragmentSerializer, object> existingFragmentSerializer
                            ? (fragment, baseSerializer, recurse) => serializer(fragment, _ => existingFragmentSerializer(_!, baseSerializer, recurse), recurse)
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
                                //return (TStatic fragment) => del((TActualType)fragment, (TActual _) => _, @this);

                                var effectiveType = del.GetType().GetGenericArguments()[0];
                                var argument2Parameter = Expression.Parameter(effectiveType);
                                var fragmentParameterExpression = Expression.Parameter(staticType);
                                var effectiveTypeFunc = typeof(Func<,>).MakeGenericType(effectiveType, typeof(object));
                                var staticTypeFunc = typeof(Func<,>).MakeGenericType(staticType, typeof(object));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    Expression.Convert(
                                        fragmentParameterExpression,
                                        effectiveType),
                                    Expression.Lambda(
                                        effectiveTypeFunc,
                                        Expression.Convert(argument2Parameter, typeof(object)),
                                        argument2Parameter),
                                    Expression.Constant(@this));

                                return Expression
                                    .Lambda(
                                        staticTypeFunc,
                                        retCall,
                                        fragmentParameterExpression)
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

        public static readonly IQueryFragmentSerializer Identity = new QueryFragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
    }
}
