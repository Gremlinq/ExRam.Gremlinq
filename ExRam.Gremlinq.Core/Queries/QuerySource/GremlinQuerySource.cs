using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.GraphElements;
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

            public ConfigurableGremlinQuerySourceImpl(string name, IGraphModel model, bool isUserSetModel, IGremlinQueryExecutor queryExecutor, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<string> excludedStrategies, ILogger logger)
            {
                Name = name;
                Model = model;
                Logger = logger;
                Executor = queryExecutor;
                _isUserSetModel = isUserSetModel;
                ExcludedStrategyNames = excludedStrategies;
                IncludedStrategies = includedStrategies;
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

            IEdgeGremlinQuery<TItem> IGremlinQuerySource.UpdateE<TItem>(TItem edge)
            {
                return Create()
                    .UpdateE(edge);
            }

            IEdgeGremlinQuery<TItem> IGremlinQuerySource.ReplaceE<TItem>(TItem edge)
            {
                return Create()
                    .ReplaceE(edge);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.UpdateV<TVertex>(TVertex vertex)
            {
                return Create()
                    .UpdateV(vertex);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.ReplaceV<TVertex>(TVertex vertex)
            {
                return Create()
                    .ReplaceV(vertex);
            }

            IVertexGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids)
            {
                return Create()
                    .V<TVertex>(ids);
            }

            IEdgeGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids)
            {
                return Create()
                    .E(ids);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids)
            {
                return Create()
                    .E<TEdge>(ids);
            }

            IGremlinQuery<TElement> IGremlinQuerySource.Inject<TElement>(params TElement[] elements)
            {
                return Create()
                    .Cast<TElement>()
                    .Inject(elements);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException($"Invalid value for {nameof(name)}.", nameof(name));

                return new ConfigurableGremlinQuerySourceImpl(name, Model, _isUserSetModel, Executor, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithLogger(ILogger logger)
            {
                var newModel = _isUserSetModel
                    ? Model
                    : GraphModel.Dynamic(NullLogger.Instance).Relax();

                return new ConfigurableGremlinQuerySourceImpl(Name, newModel, _isUserSetModel, Executor, IncludedStrategies, ExcludedStrategyNames, logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, _isUserSetModel, Executor, IncludedStrategies.AddRange(strategies), ExcludedStrategyNames, Logger);
            }

            public IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, _isUserSetModel, Executor, IncludedStrategies, ExcludedStrategyNames.AddRange(strategies), Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithModel(IGraphModel model)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, model, true, Executor, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithExecutor(IGremlinQueryExecutor executor)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, _isUserSetModel, executor, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            private IGremlinQuery Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQuery ret = GremlinQuery.Create(
                    Model,
                    Executor,
                    Name,
                    Logger);

                if (!ExcludedStrategyNames.IsEmpty)
                    ret = ret.AddStep(new WithoutStrategiesStep(ExcludedStrategyNames.ToArray()));

                foreach (var strategy in IncludedStrategies)
                {
                    ret = strategy.Apply(ret);
                }

                return Interlocked.CompareExchange(ref _startQuery, ret, null) ?? ret;
            }

            public string Name { get; }
            public ILogger Logger { get; }
            public IGraphModel Model { get; }
            public IGremlinQueryExecutor Executor { get; }
            public ImmutableList<string> ExcludedStrategyNames { get; }
            public ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
        }

        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public static readonly IConfigurableGremlinQuerySource g = Create();
#pragma warning restore IDE1006 // Naming Styles

        public static IConfigurableGremlinQuerySource Create(string name = "g")
        {
            return new ConfigurableGremlinQuerySourceImpl(name, GraphModel.Dynamic(NullLogger.Instance).Relax(), false, GremlinQueryExecutor.Invalid, ImmutableList<IGremlinQueryStrategy>.Empty, ImmutableList<string>.Empty, NullLogger.Instance);
        }

        public static IEdgeGremlinQuery<TEdge> AddE<TEdge>(this IGremlinQuerySource source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVertexGremlinQuery<TVertex> AddV<TVertex>(this IGremlinQuerySource source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }
    }
}
