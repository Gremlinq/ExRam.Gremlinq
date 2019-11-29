using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQuery
    {
        IProjectBuilder<TSourceQuery> By(string name, Func<TSourceQuery, IGremlinQuery> projection);

        IImmutableDictionary<string, IGremlinQuery> Projections { get; }
    }

    public interface IProjectBuilder<out TSourceQuery, TElement> : IProjectBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQuery<TElement>
    {
        new IProjectBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQuery> projection);
    }
}
