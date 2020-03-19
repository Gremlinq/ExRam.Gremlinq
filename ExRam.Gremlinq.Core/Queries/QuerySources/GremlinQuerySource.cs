using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        private sealed class GremlinQuerySourceImpl : IGremlinQuerySource
        {
            private IGremlinQueryBase? _startQuery;

            public GremlinQuerySourceImpl(IGremlinQueryEnvironment environment, ImmutableList<Type> excludedStrategies)
            {
                Environment = environment;
                ExcludedStrategyTypes = excludedStrategies;
            }

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            public IVertexGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new()
            {
                return Create()
                    .AddV<TVertex>();
            }

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            public IEdgeGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new()
            {
                return Create()
                    .AddE<TEdge>();
            }

            IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            public IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids)
            {
                return Create()
                    .V<TVertex>(ids);
            }

            IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids)
            {
                return Create()
                    .E(ids);
            }

            public IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids)
            {
                return Create()
                    .E<TEdge>(ids);
            }

            IGremlinQuery<TElement> IStartGremlinQuery.Inject<TElement>(params TElement[] elements)
            {
                return Create()
                    .Cast<TElement>()
                    .Inject(elements);
            }

            IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex)
            {
                var query = Create();

                return query
                    .V<TNewVertex>(vertex.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(vertex);
            }

            IEdgeGremlinQuery<TNewEdge> IStartGremlinQuery.ReplaceE<TNewEdge>(TNewEdge edge)
            {
                var query = Create();

                return query
                    .E<TNewEdge>(edge.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(edge);
            }

            IGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            {
                return new GremlinQuerySourceImpl(environmentTransformation(Environment), ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.RemoveStrategies(params Type[] strategyTypes)
            {
                return new GremlinQuerySourceImpl(Environment, ExcludedStrategyTypes.AddRange(strategyTypes));
            }

            private IGremlinQueryBase Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQueryBase ret = GremlinQuery.Create<object>(Environment);

                if (!ExcludedStrategyTypes.IsEmpty)
                    ret = ret.AddStep(new WithoutStrategiesStep(ExcludedStrategyTypes.ToArray()));

                return Interlocked.CompareExchange(ref _startQuery, ret, null) ?? ret;
            }

            public IGremlinQueryEnvironment Environment { get; }
            public ImmutableList<Type> ExcludedStrategyTypes { get; }
        }

        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IConfigurableGremlinQuerySource g = new GremlinQuerySourceImpl(
            GremlinQueryEnvironment.Default,
            ImmutableList<Type>.Empty);
        #pragma warning restore IDE1006 // Naming Styles
    }
}
