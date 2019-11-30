using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectTupleBuilder<out TSourceQuery, TElement> :
        IProjectBuilder<TSourceQuery, TElement>
        where TSourceQuery : IGremlinQuery<TElement>
    {
        new IProjectTupleBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQuery> projection);
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQuery<TElement>
    {
        new IProjectBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQuery> projection);
        new IProjectBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQuery> projection);

        IImmutableDictionary<string, IGremlinQuery> Projections { get; }
    }
}
