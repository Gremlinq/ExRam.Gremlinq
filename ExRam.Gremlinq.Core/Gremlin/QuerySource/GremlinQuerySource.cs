using System;
using System.Collections.Immutable;
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
            private readonly string _name;
            private readonly ILogger _logger;
            private readonly IGraphModel _model;
            private readonly IGremlinQueryExecutor _queryExecutor;
            private readonly ImmutableList<IGremlinQueryStrategy> _strategies;

            public ConfigurableGremlinQuerySourceImpl(string name, IGraphModel model, IGremlinQueryExecutor queryExecutor, ImmutableList<IGremlinQueryStrategy> strategies, ILogger logger)
            {
                _name = name;
                _model = model;
                _logger = logger;
                _strategies = strategies;
                _queryExecutor = queryExecutor;
            }

            IVGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex)
            {
                return Create()
                    .AddV(vertex);
            }

            IEGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge)
            {
                return Create()
                    .AddE(edge);
            }

            IVGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            IVGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids)
            {
                return Create()
                    .V<TVertex>(ids);
            }

            IEGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids)
            {
                return Create()
                    .E(ids);
            }

            IEGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids)
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

                return new ConfigurableGremlinQuerySourceImpl(name, _model, _queryExecutor, _strategies, _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithLogger(ILogger logger)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, _queryExecutor, _strategies, logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, _queryExecutor, _strategies.AddRange(strategies), _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithModel(IGraphModel model)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, model, _queryExecutor, _strategies, _logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithExecutor(IGremlinQueryExecutor executor)
            {
                return new ConfigurableGremlinQuerySourceImpl(_name, _model, executor, _strategies, _logger);
            }

            private IGremlinQuery Create()
            {
                var model = _model == GraphModel.Invalid
                    ? GraphModel.Dynamic(_logger)
                    : _model;

                var ret =
                    new GremlinQueryImpl<Unit, Unit, Unit, Unit>(
                        model,
                        _queryExecutor,
                        ImmutableList<Step>.Empty,
                        ImmutableDictionary<StepLabel, string>.Empty,
                        _logger)
                    .AddStep(IdentifierStep.Create(_name));

                foreach (var strategy in _strategies)
                {
                    ret = strategy.Apply(ret);
                }

                return ret;
            }
        }

        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IConfigurableGremlinQuerySource g = Create();
        #pragma warning restore IDE1006 // Naming Styles
    
        public static IConfigurableGremlinQuerySource Create(string name = "g")
        {
            return new ConfigurableGremlinQuerySourceImpl(name, GraphModel.Invalid, GremlinQueryExecutor.Invalid, ImmutableList<IGremlinQueryStrategy>.Empty, NullLogger.Instance);
        }

        public static IEGremlinQuery<TEdge> AddE<TEdge>(this IGremlinQuerySource source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVGremlinQuery<TVertex> AddV<TVertex>(this IGremlinQuerySource source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }
    }
}
