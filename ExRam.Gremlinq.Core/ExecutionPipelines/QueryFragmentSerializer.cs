using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public interface IQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment);

        IQueryFragmentSerializer Override<TFragment>(QueryFragmentSerializer<TFragment> serializer);
    }

    public static class QueryFragmentSerializer
    {
        private sealed class FragmentSerializerImpl : IQueryFragmentSerializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>? _fastDict;

            public FragmentSerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment)
            {
                return TryGetSerializer(typeof(TFragment), fragment.GetType()) is QueryFragmentSerializer<TFragment> del
                    ? del(fragment, _ => _, this)
                    : fragment;
            }

            public IQueryFragmentSerializer Override<TFragment>(QueryFragmentSerializer<TFragment> serializer)
            {
                return new FragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        _dict.TryGetValue(typeof(TFragment), out var existingAtomSerializer)
                            ? new QueryFragmentSerializer<TFragment>((atom, baseSerializer, recurse) => serializer(atom, _ => ((QueryFragmentSerializer<TFragment>)existingAtomSerializer)(_!, baseSerializer, recurse), recurse))
                            : (atom, baseSerializer, recurse) => serializer(atom, _ => baseSerializer(_!), recurse)));
            }

            private Delegate? TryGetSerializer(Type staticType, Type actualType)
            {
                var fastDict = Volatile.Read(ref _fastDict);
                if (fastDict == null)
                    Volatile.Write(ref _fastDict, fastDict = new ConcurrentDictionary<(Type, Type), Delegate?>());

                return fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        (typeTuple, @this) =>
                        {
                            Delegate InnerLookup(Type staticType, Type actualType)
                            {
                                if (@this._dict.TryGetValue(actualType, out var ret))
                                    return ret;

                                foreach (var implementedInterface in actualType.GetInterfaces())
                                {
                                    if (InnerLookup(staticType, implementedInterface) is { } interfaceSerializer)
                                        return interfaceSerializer;
                                }

                                if (actualType.BaseType is { } baseType)
                                {
                                    if (InnerLookup(staticType, baseType) is { } baseSerializer)
                                        return baseSerializer;
                                }

                                return null;
                            }

                            if (InnerLookup(typeTuple.staticType, typeTuple.actualType) is { } del)
                            {
                                var effectiveType = del.GetType().GetGenericArguments()[0];

                                if (effectiveType == typeTuple.staticType)
                                    return del;

                                //Func<TStatic, object>
                                var staticBaseSerializerType = typeof(Func<,>).MakeGenericType(staticType, typeof(object));

                                //Func<TActual, object>
                                var actualBaseSerializerType = typeof(Func<,>).MakeGenericType(effectiveType, typeof(object));

                                //return (TStatic fragment, Func<TStatic, object> baseSerializer, IQueryFragmentSerializer recurse) => ret((TActualType)fragment, (TActual _) => baseSerializer((TStatic)_), recurse);

                                var staticQuerySerializerType = typeof(QueryFragmentSerializer<>).MakeGenericType(staticType);

                                var fragmentParameterExpression = Expression.Parameter(staticType);
                                var baseSerializerParameterExpression = Expression.Parameter(staticBaseSerializerType);
                                var recurseParameter = Expression.Parameter(typeof(IQueryFragmentSerializer));

                                //(TActualType)fragment
                                var argument1 = Expression.Convert(fragmentParameterExpression, effectiveType);

                                //(TActual _) => baseSerializer((TFragment)_)
                                var argument2Parameter = Expression.Parameter(effectiveType);
                                var argument2 = Expression.Lambda(
                                    actualBaseSerializerType,
                                    Expression.Invoke(
                                        baseSerializerParameterExpression,
                                        Expression.Convert(argument2Parameter, staticType)),
                                    argument2Parameter);

                                var retCall = Expression.Invoke(Expression.Constant(del), argument1, argument2, recurseParameter);

                                return Expression
                                    .Lambda(staticQuerySerializerType, retCall, fragmentParameterExpression, baseSerializerParameterExpression, recurseParameter)
                                    .Compile();
                            }

                            return null;
                        },
                        this);
            }
        }

        public static readonly IQueryFragmentSerializer Identity = new FragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
    }

    public delegate object QueryFragmentSerializer<TFragment>(TFragment fragment, Func<TFragment, object> baseSerializer, IQueryFragmentSerializer recurse);
}
