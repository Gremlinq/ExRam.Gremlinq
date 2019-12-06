using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        private sealed class GremlinQuerySourceImpl : IGremlinQuerySource
        {
            private IGremlinQuery _startQuery;

            public GremlinQuerySourceImpl(string name, IGremlinQueryEnvironment environment, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<Type> excludedStrategies)
            {
                Name = name;
                Environment = environment;
                IncludedStrategies = includedStrategies;
                ExcludedStrategyTypes = excludedStrategies;
            }

            IVertexGremlinQuery<TVertex> IGremlinQueryBase.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQueryBase.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            IVertexGremlinQuery<IVertex> IGremlinQueryBase.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            IEdgeGremlinQuery<IEdge> IGremlinQueryBase.E(params object[] ids)
            {
                return Create()
                    .E(ids);
            }

            IGremlinQuery<TElement> IGremlinQueryBase.Inject<TElement>(params TElement[] elements)
            {
                return Create()
                    .Cast<TElement>()
                    .Inject(elements);
            }

            IVertexGremlinQuery<TNewVertex> IGremlinQueryBase.ReplaceV<TNewVertex>(TNewVertex vertex)
            {
                var query = Create();

                return query
                    .V<TNewVertex>(vertex.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(vertex);
            }

            IEdgeGremlinQuery<TNewEdge> IGremlinQueryBase.ReplaceE<TNewEdge>(TNewEdge edge)
            {
                var query = Create();

                return query
                    .E<TNewEdge>(edge.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(edge);
            }

            IGremlinQuerySource IGremlinQuerySource.UseName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException($"Invalid value for {nameof(name)}.", nameof(name));

                return new GremlinQuerySourceImpl(name, Environment, IncludedStrategies, ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            {
                return new GremlinQuerySourceImpl(Name, environmentTransformation(Environment), IncludedStrategies, ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.AddStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new GremlinQuerySourceImpl(Name, Environment, IncludedStrategies.AddRange(strategies), ExcludedStrategyTypes);
            }

            IGremlinQuerySource IGremlinQuerySource.RemoveStrategies(params Type[] strategyTypes)
            {
                return new GremlinQuerySourceImpl(Name, Environment, IncludedStrategies, ExcludedStrategyTypes.AddRange(strategyTypes));
            }

            private IGremlinQuery Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQuery ret = GremlinQuery.Create<Unit>(Environment);

                if (!ExcludedStrategyTypes.IsEmpty)
                    ret = ret.AddStep(new WithoutStrategiesStep(ExcludedStrategyTypes.ToArray()));

                foreach (var strategy in IncludedStrategies)
                {
                    ret = strategy.Apply(ret);
                }

                return Interlocked.CompareExchange(ref _startQuery, ret, null) ?? ret;
            }

            public string Name { get; }
            public IGremlinQueryEnvironment Environment { get; }
            public ImmutableList<Type> ExcludedStrategyTypes { get; }
            public ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
        }

        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IGremlinQuerySource g = Create();
        #pragma warning restore IDE1006 // Naming Styles
    
        public static IGremlinQuerySource Create(string name = "g")
        {
            return new GremlinQuerySourceImpl(
                name,
                GremlinQueryEnvironment.Default,
                ImmutableList<IGremlinQueryStrategy>.Empty,
                ImmutableList<Type>.Empty);
        }

        public static IEdgeGremlinQuery<TEdge> AddE<TEdge>(this IGremlinQueryBase source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVertexGremlinQuery<TVertex> AddV<TVertex>(this IGremlinQueryBase source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }

        public static IEdgeGremlinQuery<TEdge> E<TEdge>(this IGremlinQueryBase source, params object[] ids)
        {
            return source.E(ids).OfType<TEdge>();
        }

        public static IVertexGremlinQuery<TVertex> V<TVertex>(this IGremlinQueryBase source, params object[] ids)
        {
            return source.V(ids).OfType<TVertex>();
        }
    }
}
