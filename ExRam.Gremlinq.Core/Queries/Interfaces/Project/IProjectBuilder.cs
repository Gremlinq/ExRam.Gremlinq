using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectResult
    {
        IImmutableDictionary<string, IGremlinQueryBase> Projections { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectResult<TResult> : IProjectResult
    {
        
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(Func<TSourceQuery, IGremlinQueryBase<TItem1>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(Func<TSourceQuery, IGremlinQueryBase<TItem2>> projection);
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectResult
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);
    }
}
