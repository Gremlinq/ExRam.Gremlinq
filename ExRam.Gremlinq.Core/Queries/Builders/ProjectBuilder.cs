using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    internal static class ProjectBuilder
    {
        private sealed class ProjectBuilderImpl<TSourceQuery, TElement> : IProjectBuilder<TSourceQuery, TElement> where TSourceQuery : IGremlinQuery<TElement>
        {
            private readonly TSourceQuery _sourceQuery;

            public ProjectBuilderImpl(TSourceQuery sourceQuery, IImmutableDictionary<string, IGremlinQuery> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }

            public IProjectBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return new ProjectBuilderImpl<TSourceQuery, TElement>(
                    _sourceQuery,
                    Projections.SetItem(name, projection(_sourceQuery)));
            }

            IProjectBuilder<TSourceQuery> IProjectBuilder<TSourceQuery>.By(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return By(name, projection);
            }

            public IImmutableDictionary<string, IGremlinQuery> Projections { get; }
        }

        public static IProjectBuilder<TSourceQuery, TElement> Create<TSourceQuery, TElement>(TSourceQuery sourceQuery) where TSourceQuery : IGremlinQuery<TElement>
        {
            return new ProjectBuilderImpl<TSourceQuery, TElement>(sourceQuery, ImmutableDictionary<string, IGremlinQuery>.Empty);
        }
    }
}
