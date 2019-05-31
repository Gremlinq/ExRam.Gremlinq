using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Serialization;
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

            public ConfigurableGremlinQuerySourceImpl(string name, IGraphModel model, Options options, bool isUserSetModel, IGremlinQueryExecutor queryExecutor, IGremlinQueryElementVisitorCollection visitors, ImmutableList<IGremlinQueryStrategy> includedStrategies, ImmutableList<string> excludedStrategies, ILogger logger)
            {
                Name = name;
                Model = model;
                Logger = logger;
                Options = options;
                Visitors = visitors;
                Executor = queryExecutor;
                _isUserSetModel = isUserSetModel;
                IncludedStrategies = includedStrategies;
                ExcludedStrategyNames = excludedStrategies;
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

                return new ConfigurableGremlinQuerySourceImpl(name, Model, Options, _isUserSetModel, Executor, Visitors, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithLogger(ILogger logger)
            {
                var newModel = _isUserSetModel
                    ? Model
                    : GraphModel.Dynamic(NullLogger.Instance);

                return new ConfigurableGremlinQuerySourceImpl(Name, newModel, Options, _isUserSetModel, Executor, Visitors, IncludedStrategies, ExcludedStrategyNames, logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.WithStrategies(params IGremlinQueryStrategy[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, Options, _isUserSetModel, Executor, Visitors, IncludedStrategies.AddRange(strategies), ExcludedStrategyNames, Logger);
            }

            public IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, Options, _isUserSetModel, Executor, Visitors, IncludedStrategies, ExcludedStrategyNames.AddRange(strategies), Logger);
            }

            public IConfigurableGremlinQuerySource ConfigureOptions(Func<Options, Options> optionsTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, optionsTransformation(Options), _isUserSetModel, Executor, Visitors, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, modelTransformation(Model), Options, true, Executor, Visitors, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, Options, _isUserSetModel, executorTransformation(Executor), Visitors, IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            IConfigurableGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureVisitors(Func<IGremlinQueryElementVisitorCollection, IGremlinQueryElementVisitorCollection> visitorsTransformation)
            {
                return new ConfigurableGremlinQuerySourceImpl(Name, Model, Options, _isUserSetModel, Executor, visitorsTransformation(Visitors), IncludedStrategies, ExcludedStrategyNames, Logger);
            }

            private IGremlinQuery Create()
            {
                var startQuery = Volatile.Read(ref _startQuery);
                if (startQuery != null)
                    return startQuery;

                IGremlinQuery ret = GremlinQuery.Create(
                    Name,
                    this);

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
            public Options Options { get; }
            public IGraphModel Model { get; }
            public IGremlinQueryExecutor Executor { get; }
            public ImmutableList<string> ExcludedStrategyNames { get; }
            public IGremlinQueryElementVisitorCollection Visitors { get; }
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
                GraphModel.Dynamic(NullLogger.Instance),
                default,
                false,
                GremlinQueryExecutor.Invalid,
                GremlinQueryElementVisitorCollection.Default,
                ImmutableList<IGremlinQueryStrategy>.Empty,
                ImmutableList<string>.Empty,
                NullLogger.Instance);
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
