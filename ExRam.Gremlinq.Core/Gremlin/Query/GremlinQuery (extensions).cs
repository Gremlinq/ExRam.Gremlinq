using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuery
    {
        internal static IGremlinQuery AddStep(this IGremlinQuery query, Step step)
        {
            return query.AsAdmin().InsertStep(query.AsAdmin().Steps.Count, step);
        }

        public static IGremlinQuery<Unit> Anonymous(IGraphModel model, ILogger logger = null)
        {
            return Create(model, GremlinQueryExecutor.Invalid, null, logger);
        }

        public static IVertexGremlinQuery<TVertex> BothV<TVertex>(this IEdgeGremlinQuery query)
        {
            return query
                .BothV()
                .OfType<TVertex>();
        }

        internal static IGremlinQuery<Unit> Create(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return Create<Unit>(model, queryExecutor, graphName, logger);
        }

        internal static IGremlinQuery<TElement> Create<TElement>(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return new GremlinQuery<TElement, Unit, Unit, Unit, Unit>(
                model,
                queryExecutor,
                graphName != null
                    ? ImmutableList<Step>.Empty.Add(IdentifierStep.Create(graphName))
                    : ImmutableList<Step>.Empty,
                ImmutableDictionary<StepLabel, string>.Empty,
                logger);
        }

        public static IGremlinQuery<TEdge> E<TEdge>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TEdge>();
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

        public static IVertexGremlinQuery<TVertex> InV<TVertex>(this IEdgeGremlinQuery query)
        {
            return query
                .InV()
                .OfType<TVertex>();
        }

        public static IVertexGremlinQuery<TVertex> OtherV<TVertex>(this IEdgeGremlinQuery query)
        {
            return query
                .OtherV()
                .OfType<TVertex>();
        }

        public static IVertexGremlinQuery<TVertex> OutV<TVertex>(this IEdgeGremlinQuery query)
        {
            return query
                .OutV()
                .OfType<TVertex>();
        }

        public static IVertexPropertyGremlinQuery<object> Properties<TVertex>(this IVertexGremlinQuery<TVertex> query, params Expression<Func<TVertex, object>>[] projections)
        {
            return query.Properties(projections);
        }

        public static IEdgePropertyGremlinQuery<object> Properties<TEdge>(this IEdgeGremlinQuery<TEdge> query, params Expression<Func<TEdge, object>>[] projections)
        {
            return query.Properties(projections);
        }

        public static IGremlinQuery<Property<object>> Properties<TValue>(this IVertexPropertyGremlinQuery<TValue> query, params string[] keys)
        {
            return query.Properties<object>(keys);
        }

        public static IVertexGremlinQuery<TVertex> V<TVertex>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TVertex>();
        }

        public static IGremlinQuery<object> Values<TValue>(this IVertexPropertyGremlinQuery<TValue> query)
        {
            return query.Values<object>();
        }

        public static IGremlinQuery<object> Values<TValue, TMeta>(this IVertexPropertyGremlinQuery<TValue, TMeta> query)
        {
            return query.Values<object>();
        }
    }
}
