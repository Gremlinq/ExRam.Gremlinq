using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectResult
    {
        IImmutableDictionary<string, IGremlinQuery> Projections { get; }
    }

    public interface IProjectResult<TResult> : IProjectResult
    {
        
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQuery
    {
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement> : IProjectResult<object>
        where TSourceQuery : IGremlinQuery
    {
        IProjectTupleBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQuery> projection);
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectResult
        where TSourceQuery : IGremlinQuery
    {
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQuery> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQuery> projection);
    }
}
