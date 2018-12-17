using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ExRam.Gremlinq
{
    public static class GremlinQuery
    {
        internal static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static IGremlinQuery<Unit> Anonymous(IGraphModel model)
        {
            return Create(model, GremlinQueryProvider.Invalid, "__");
        }

        internal static IGremlinQuery<Unit> Create(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName)
        {
            return Create<Unit>(model, queryExecutor, graphName);
        }

        internal static IGremlinQuery<TElement> Create<TElement>(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = "g")
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(model, queryExecutor,  ImmutableList<Step>.Empty.Add(new IdentifierStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }
        
        public static Task<TElement> First<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            return AsyncEnumerable.First(
                query.Limit(1),
                ct);
        }

        public static async Task<Option<TElement>> FirstOrNone<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            var array = await query
                .Limit(1)
                .ToArray(ct)
                .ConfigureAwait(false);

            return array.Length > 0
                ? array[0]
                : Option<TElement>.None;
        }

        internal static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery<TElement> query, Step step)
        {
            return query.InsertStep<TElement>(query.Steps.Count, step);
        }
        
        public static IGremlinQuery<TEdge> E<TEdge>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TEdge>();
        }

        public static IVGremlinQuery<TVertex> V<TVertex>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TVertex>();
        }

        public static IGremlinQuery<TElement> Unfold<TElement>(this IGremlinQuery<TElement[]> query)
        {
            return query.Unfold<TElement>();
        }
    }
}
