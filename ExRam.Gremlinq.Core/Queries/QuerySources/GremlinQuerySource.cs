using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        private sealed class ConfigurableGremlinQuerySourceImpl : IConfigurableGremlinQuerySource
        {
            private IGremlinQuery _startQuery;
            private readonly bool _isUserSetModel;

            public ConfigurableGremlinQuerySourceImpl(string name, IGremlinQueryEnvironment environment, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<Type> excludedStrategies)
            {
                Name = name;
                Environment = environment;
                //_isUserSetModel = isUserSetModel;
                IncludedStrategies = includedStrategies;
                ExcludedStrategyTypes = excludedStrategies;
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            IVertexGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            IEdgeGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids)
            {
                return Create()
                    .E(ids);
            }

            IGremlinQuery<TElement> IGremlinQuerySource.Inject<TElement>(params TElement[] elements)
            {
                return Create()
                    .Cast<TElement>()
                    .Inject(elements);
            }

            IVertexGremlinQuery<TNewVertex> IGremlinQuerySource.ReplaceV<TNewVertex>(TNewVertex vertex)
            {
                var query = Create();

                return query
                    .V<TNewVertex>(vertex.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(vertex);
            }

            IEdgeGremlinQuery<TNewEdge> IGremlinQuerySource.ReplaceE<TNewEdge>(TNewEdge edge)
            {
                var query = Create();

                return query
                    .E<TNewEdge>(edge.GetId(query.AsAdmin().Environment.Model.PropertiesModel))
                    .Update(edge);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.UseName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException($"Invalid value for {nameof(name)}.", nameof(name));

                return new ConfigurableGremlinQuerySourceImpl(name, Environment, IncludedStrategies, ExcludedStrategyTypes);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, environmentTransformation(Environment), IncludedStrategies, ExcludedStrategyTypes);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.AddStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Environment, IncludedStrategies.AddRange(strategies), ExcludedStrategyTypes);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.RemoveStrategies(params Type[] strategyTypes)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Environment, IncludedStrategies, ExcludedStrategyTypes.AddRange(strategyTypes));
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
        public static readonly IConfigurableGremlinQuerySource g = Create();
        #pragma warning restore IDE1006 // Naming Styles
    
        public static IConfigurableGremlinQuerySource Create(string name = "g")
        {
            return new ConfigurableGremlinQuerySourceImpl(
                name,
                GremlinQueryEnvironment.Default,
                ImmutableList<IGremlinQueryStrategy>.Empty,
                ImmutableList<Type>.Empty);
        }

        public static IEdgeGremlinQuery<TEdge> AddE<TEdge>(this IGremlinQuerySource source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVertexGremlinQuery<TVertex> AddV<TVertex>(this IGremlinQuerySource source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }

        public static IEdgeGremlinQuery<TEdge> E<TEdge>(this IGremlinQuerySource source, params object[] ids)
        {
            return source.E(ids).OfType<TEdge>();
        }

        public static IVertexGremlinQuery<TVertex> V<TVertex>(this IGremlinQuerySource source, params object[] ids)
        {
            return source.V(ids).OfType<TVertex>();
        }
    }
}
