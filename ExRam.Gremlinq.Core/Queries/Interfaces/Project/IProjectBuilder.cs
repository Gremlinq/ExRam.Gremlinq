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
}
