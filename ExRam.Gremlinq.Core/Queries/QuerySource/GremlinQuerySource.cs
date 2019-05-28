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
                    : GraphModel.Dynamic(NullLogger.Instance);

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

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, modelTransformation(Model), true, Executor, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, _isUserSetModel, executorTransformation(Executor), IncludedStrategies, ExcludedStrategyNames, Logger);
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
            return new ConfigurableGremlinQuerySourceImpl(name, GraphModel.Dynamic(NullLogger.Instance), false, GremlinQueryExecutor.Invalid, ImmutableList<IGremlinQueryStrategy>.Empty, ImmutableList<string>.Empty, NullLogger.Instance);
        }

        public static IEdgeGremlinQuery<TEdge> AddE<TEdge>(this IGremlinQuerySource source) where TEdge : new()
        {
            return source.AddE(new TEdge());
        }

        public static IVertexGremlinQuery<TVertex> AddV<TVertex>(this IGremlinQuerySource source) where TVertex : new()
        {
            return source.AddV(new TVertex());
        }

        public static IConfigurableGremlinQuerySource WithModel(this IConfigurableGremlinQuerySource source, IGraphModel model)
        {
            return source.ConfigureModel(_ => model);
        }

        public static IConfigurableGremlinQuerySource WithExecutor(this IConfigurableGremlinQuerySource source, IGremlinQueryExecutor executor)
        {
            return source.ConfigureExecutor(_ => executor);
        }

        public static IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(this IGremlinQuerySource source, TNewVertex vertex)
        {
            return source
                .V<TNewVertex>(vertex.GetId())
                .Update(vertex);
        }

        public static IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(this IGremlinQuerySource source, TNewEdge edge)
        {
            return source
                .E<TNewEdge>(edge.GetId())
                .Update(edge);
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
