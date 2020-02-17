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

            public GremlinQuerySourceImpl(IGremlinQueryEnvironment environment, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<Type> excludedStrategies)
            {
                Environment = environment;
                IncludedStrategies = includedStrategies;
                ExcludedStrategyTypes = excludedStrategies;
            }

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids)
            {
                return Create()
                    .E(ids);
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
                return new GremlinQuerySourceImpl(environmentTransformation(Environment), IncludedStrategies, ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.AddStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new GremlinQuerySourceImpl(Environment, IncludedStrategies.AddRange(strategies), ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.RemoveStrategies(params Type[] strategyTypes)
            {
                return new GremlinQuerySourceImpl(Environment, IncludedStrategies, ExcludedStrategyTypes.AddRange(strategyTypes));
            }

            private IGremlinQueryBase Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQueryBase ret = GremlinQuery.Create<object>(Environment);

                if (!ExcludedStrategyTypes.IsEmpty)
                    ret = ret.AddStep(new WithoutStrategiesStep(ExcludedStrategyTypes.ToArray()));

                foreach (var strategy in IncludedStrategies)
                {
                    ret = strategy.Apply(ret);
                }

                return Interlocked.CompareExchange(ref _startQuery, ret, null) ?? ret;
            }

            public IGremlinQueryEnvironment Environment { get; }
            public ImmutableList<Type> ExcludedStrategyTypes { get; }
            public ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
        }

        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IConfigurableGremlinQuerySource g = new GremlinQuerySourceImpl(
            GremlinQueryEnvironment.Default,
            ImmutableList<IGremlinQueryStrategy>.Empty,
            ImmutableList<Type>.Empty);
        #pragma warning restore IDE1006 // Naming Styles

        public static IEdgeGremlinQuery<TEdge> AddE<TEdge>(this IStartGremlinQuery source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVertexGremlinQuery<TVertex> AddV<TVertex>(this IStartGremlinQuery source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }

        public static IEdgeGremlinQuery<TEdge> E<TEdge>(this IStartGremlinQuery source, params object[] ids)
        {
            return source.E(ids).OfType<TEdge>();
        }

        public static IVertexGremlinQuery<TVertex> V<TVertex>(this IStartGremlinQuery source, params object[] ids)
        {
            return source.V(ids).OfType<TVertex>();
        }
    }
}
