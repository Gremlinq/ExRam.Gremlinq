using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuery
    {
        public static IGremlinQuery<Unit> Anonymous(IGraphModel model, ILogger logger = null)
        {
            return Create(model, GremlinQueryExecutor.Invalid, null, logger);
        }

        internal static IGremlinQuery<Unit> Create(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return Create<Unit>(model, queryExecutor, graphName, logger);
        }

        internal static IGremlinQuery<TElement> Create<TElement>(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return new GremlinQueryImpl<TElement, Unit, Unit, Unit, Unit>(
                model,
                queryExecutor,
                graphName != null
                    ? ImmutableList<Step>.Empty.Add(IdentifierStep.Create(graphName))
                    : ImmutableList<Step>.Empty,
                ImmutableDictionary<StepLabel, string>.Empty,
                logger);
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

        internal static IGremlinQuery AddStep(this IGremlinQuery query, Step step)
        {
            return query.AsAdmin().InsertStep(query.AsAdmin().Steps.Count, step);
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

        public static IVPropertiesGremlinQuery<VertexProperty<object>, object> Properties<TVertex>(this IVGremlinQuery<TVertex> query, params Expression<Func<TVertex, object>>[] projections)
        {
            return query.Properties(projections);
        }

        public static IGremlinQuery<object> Values<TMeta>(this IVPropertiesGremlinQuery<VertexProperty<object, TMeta>, object, TMeta> query)
        {
            return query.Values<object>();
        }
    }
}
