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
            private readonly string _name;
            private readonly ILogger _logger;
            private readonly IGraphModel _model;
            private readonly bool _isUserSetModel;
            private readonly IGremlinQueryExecutor _queryExecutor;
            private readonly ImmutableList<string> _excludedStrategies;
            private readonly ImmutableList<IGremlinQueryStrategy> _includedStrategies;

            private IGremlinQuery _startQuery;

            public ConfigurableGremlinQuerySourceImpl(string name, IGraphModel model, bool isUserSetModel, IGremlinQueryExecutor queryExecutor, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<string> excludedStrategies, ILogger logger)
            {
                _name = name;
                _model = model;
                _logger = logger;
                _queryExecutor = queryExecutor;
                _isUserSetModel = isUserSetModel;
                _excludedStrategies = excludedStrategies;
                _includedStrategies = includedStrategies;
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.UpdateV<TVertex>(TVertex vertex)
            {
                return Create()
                    .UpdateV(vertex);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.UpdateV<TVertex>(TVertex vertex, Func<string, bool> excludePropertyFilter)
            {
                return Create()
                    .UpdateV(vertex, excludePropertyFilter);
            }

            IVertexGremlinQuery<TVertex> IGremlinQuerySource.UpdateV<TVertex>(TVertex vertex, string[] excludeFromUpdate)
            {
                return Create()
                    .UpdateV(vertex, excludeFromUpdate);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.UpdateE<TEdge>(TEdge edge)
            {
                return Create()
                    .UpdateE(edge);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.UpdateE<TEdge>(TEdge edge, Func<string, bool> excludePropertyFilter)
            {
                return Create()
                    .UpdateE(edge, excludePropertyFilter);
            }

            IEdgeGremlinQuery<TEdge> IGremlinQuerySource.UpdateE<TEdge>(TEdge edge, string[] excludeFromUpdate)
            {
                return Create()
                    .UpdateE(edge, excludeFromUpdate);
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

                return new ConfigurableGremlinQuerySourceImpl(name, _model, _isUserSetModel, _queryExecutor, _includedStrategies, _excludedStrategies, _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithLogger(ILogger logger)
            {
                var newModel = _isUserSetModel
                    ? _model
                    : GraphModel.Dynamic(NullLogger.Instance).Relax();

                return new ConfigurableGremlinQuerySourceImpl(_name, newModel, _isUserSetModel, _queryExecutor, _includedStrategies, _excludedStrategies, logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, _isUserSetModel, _queryExecutor, _includedStrategies.AddRange(strategies), _excludedStrategies, _logger);
            }

            public IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, _isUserSetModel, _queryExecutor, _includedStrategies, _excludedStrategies.AddRange(strategies), _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithModel(IGraphModel model)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, model, true, _queryExecutor, _includedStrategies, _excludedStrategies, _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithExecutor(IGremlinQueryExecutor executor)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, _isUserSetModel, executor, _includedStrategies, _excludedStrategies, _logger);
            }

            private IGremlinQuery Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQuery ret = GremlinQuery.Create(
                    _model,
                    _queryExecutor,
                    _name,
                    _logger);

                if (!_excludedStrategies.IsEmpty)
                    ret = ret.AddStep(new WithoutStrategiesStep(_excludedStrategies.ToArray()));

                foreach (var strategy in _includedStrategies)
                {
                    ret = strategy.Apply(ret);
                }

                return Interlocked.CompareExchange(ref _startQuery, ret, null) ?? ret;
            }
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
