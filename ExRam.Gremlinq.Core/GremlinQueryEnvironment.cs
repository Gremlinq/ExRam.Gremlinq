using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironment
    {
        private sealed class GremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public GremlinQueryEnvironmentImpl(IGraphModel model, IGremlinQueryExecutionPipeline pipeline, IImmutableDictionary<GremlinqOption, object> options, ILogger logger)
            {
                Model = model;
                Pipeline = pipeline;
                Options = options;
                Logger = logger;
            }

            public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation)
            {
                return new GremlinQueryEnvironmentImpl(modelTransformation(Model), Pipeline, Options, Logger);
            }

            public IGremlinQueryEnvironment ConfigureExecutionPipeline(Func<IGremlinQueryExecutionPipeline, IGremlinQueryExecutionPipeline> pipelineTransformation)
            {
                return new GremlinQueryEnvironmentImpl(Model, pipelineTransformation(Pipeline), Options, Logger);
            }

            public IGremlinQueryEnvironment ConfigureOptions(Func<IImmutableDictionary<GremlinqOption, object>, IImmutableDictionary<GremlinqOption, object>> optionsTransformation)
            {
                return new GremlinQueryEnvironmentImpl(Model, Pipeline, optionsTransformation(Options), Logger);
            }

            public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation)
            {
                return new GremlinQueryEnvironmentImpl(Model, Pipeline, Options, loggerTransformation(Logger));
            }

            public ILogger Logger { get; }
            public IGraphModel Model { get; }
            public IGremlinQueryExecutionPipeline Pipeline { get; }
            public IImmutableDictionary<GremlinqOption, object> Options { get; }
        }

        public static IGremlinQueryEnvironment UseModel(this IGremlinQueryEnvironment source, IGraphModel model)
        {
            return source.ConfigureModel(_ => model);
        }

        public static IGremlinQueryEnvironment UseExecutionPipeline(this IGremlinQueryEnvironment source, IGremlinQueryExecutionPipeline pipeline)
        {
            return source.ConfigureExecutionPipeline(_ => pipeline);
        }

        public static IGremlinQueryEnvironment UseLogger(this IGremlinQueryEnvironment source, ILogger logger)
        {
            return source.ConfigureLogger(_ => logger);
        }

        public static readonly IGremlinQueryEnvironment Empty = new GremlinQueryEnvironmentImpl(
            GraphModel.Empty,
            GremlinQueryExecutionPipeline.Empty,
            ImmutableDictionary<GremlinqOption, object>.Empty,
            NullLogger.Instance);

        public static readonly IGremlinQueryEnvironment Default = new GremlinQueryEnvironmentImpl(
            GraphModel.Default(lookup => lookup
                .IncludeAssembliesFromAppDomain()),
            GremlinQueryExecutionPipeline.Empty
                .UseSerializer(GremlinQuerySerializer.Default),
            ImmutableDictionary<GremlinqOption, object>.Empty,
            NullLogger.Instance);

        internal static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(Empty).Identity());
    }
}
