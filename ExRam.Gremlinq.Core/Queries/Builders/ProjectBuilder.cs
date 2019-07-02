using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal static class ProjectBuilder
    {
        private sealed class ProjectBuilderImpl<TSourceQuery> : IProjectBuilder<TSourceQuery> where TSourceQuery : IGremlinQuery
        {
            private readonly TSourceQuery _sourceQuery;

            public ProjectBuilderImpl(TSourceQuery sourceQuery, IImmutableDictionary<string, IGremlinQuery> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }

            public IProjectBuilder<TSourceQuery> By(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return new ProjectBuilderImpl<TSourceQuery>(
                    _sourceQuery,
                    Projections.SetItem(name, projection(_sourceQuery)));
            }

            public IImmutableDictionary<string, IGremlinQuery> Projections { get; }
        }

        public static IProjectBuilder<TSourceQuery> Create<TSourceQuery>(TSourceQuery sourceQuery) where TSourceQuery : IGremlinQuery
        {
            return new ProjectBuilderImpl<TSourceQuery>(sourceQuery, ImmutableDictionary<string, IGremlinQuery>.Empty);
        }
    }
}
