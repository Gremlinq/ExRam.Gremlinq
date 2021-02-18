using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironment
    {
        private sealed class GremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public GremlinQueryEnvironmentImpl(
                IGraphModel model,
                IAddStepHandler addStepHandler,
                IGremlinQuerySerializer serializer,
                IGremlinQueryExecutor executor,
                IGremlinQueryExecutionResultDeserializer deserializer,
                IFeatureSet featureSet,
                IGremlinqOptions options,
                ILogger logger)
            {
                Model = model;
                Logger = logger;
                Options = options;
                Executor = executor;
                FeatureSet = featureSet;
                Serializer = serializer;
                Deserializer = deserializer;
                AddStepHandler = addStepHandler;
            }

            public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => new GremlinQueryEnvironmentImpl(modelTransformation(Model), AddStepHandler, Serializer, Executor, Deserializer, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, Serializer, Executor, Deserializer, FeatureSet, optionsTransformation(Options), Logger);

            public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, Serializer, Executor, Deserializer, featureSetTransformation(FeatureSet), Options, Logger);

            public IGremlinQueryEnvironment ConfigureAddStepHandler(Func<IAddStepHandler, IAddStepHandler> handlerTransformation) => new GremlinQueryEnvironmentImpl(Model, handlerTransformation(AddStepHandler), Serializer, Executor, Deserializer, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, Serializer, Executor, Deserializer, FeatureSet, Options, loggerTransformation(Logger));

            public IGremlinQueryEnvironment ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer, IGremlinQueryExecutionResultDeserializer> configurator) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, Serializer, Executor, configurator(Deserializer), FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureSerializer(Func<IGremlinQuerySerializer, IGremlinQuerySerializer> configurator) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, configurator(Serializer), Executor, Deserializer, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> configurator) => new GremlinQueryEnvironmentImpl(Model, AddStepHandler, Serializer, configurator(Executor), Deserializer, FeatureSet, Options, Logger);

            public ILogger Logger { get; }
            public IGraphModel Model { get; }
            public IFeatureSet FeatureSet { get; }
            public IAddStepHandler AddStepHandler { get; }
            public IGremlinQueryExecutor Executor { get; }
            public IGremlinQuerySerializer Serializer { get; }
            public IGremlinQueryExecutionResultDeserializer Deserializer { get; }
            public IGremlinqOptions Options { get; }
        }

        public static readonly IGremlinQueryEnvironment Empty = new GremlinQueryEnvironmentImpl(
            GraphModel.Empty,
            AddStepHandler.Empty,
            GremlinQuerySerializer.Identity,
            GremlinQueryExecutor.Empty,
            GremlinQueryExecutionResultDeserializer.Identity,
            FeatureSet.Full,
            GremlinqOptions.Empty,
            NullLogger.Instance);

        public static readonly IGremlinQueryEnvironment Default = Empty
            .UseAddStepHandler(AddStepHandler.Default)
            .UseModel(GraphModel.Default(lookup => lookup
                .IncludeAssembliesFromAppDomain()))
            .UseSerializer(GremlinQuerySerializer.Default)
            .UseExecutor(GremlinQueryExecutor.Invalid)
            .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default);

        internal static readonly Step NoneWorkaround = new NotStep(IdentityStep.Instance);

        public static IGremlinQueryEnvironment UseAddStepHandler(this IGremlinQueryEnvironment source, IAddStepHandler addStepHandler) => source.ConfigureAddStepHandler(_ => addStepHandler);

        public static IGremlinQueryEnvironment UseModel(this IGremlinQueryEnvironment source, IGraphModel model) => source.ConfigureModel(_ => model);

        public static IGremlinQueryEnvironment UseLogger(this IGremlinQueryEnvironment source, ILogger logger) => source.ConfigureLogger(_ => logger);

        public static IGremlinQueryEnvironment UseSerializer(this IGremlinQueryEnvironment environment, IGremlinQuerySerializer serializer) => environment.ConfigureSerializer(_ => serializer);

        public static IGremlinQueryEnvironment UseDeserializer(this IGremlinQueryEnvironment environment, IGremlinQueryExecutionResultDeserializer deserializer) => environment.ConfigureDeserializer(_ => deserializer);

        public static IGremlinQueryEnvironment UseExecutor(this IGremlinQueryEnvironment environment, IGremlinQueryExecutor executor) => environment.ConfigureExecutor(_ => executor);

        public static IGremlinQueryEnvironment EchoGraphsonString(this IGremlinQueryEnvironment environment)
        {
            return environment
                .UseSerializer(GremlinQuerySerializer.Default)
                .UseExecutor(GremlinQueryExecutor.Identity)
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.ToGraphsonString);
        }

        public static IGremlinQueryEnvironment EchoGroovyString(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureSerializer(serializer => serializer.ToGroovy())
                .UseExecutor(GremlinQueryExecutor.Identity)
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default);
        }

        public static IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureSerializer(serializer => serializer
                    .ConfigureFragmentSerializer(fragmentSerializer =>  fragmentSerializer
                        .Override<TimeSpan>((t, env, _, recurse) => recurse.Serialize(t.TotalMilliseconds, env))))
                .ConfigureDeserializer(deserializer => deserializer
                    .ConfigureFragmentDeserializer(fragmentDeserializer => fragmentDeserializer
                        .Override<JValue>((jValue, type, env, overridden, recurse) => type == typeof(TimeSpan)
                            ? TimeSpan.FromMilliseconds(jValue.Value<double>())
                            : overridden(jValue, type, env, recurse))));
        }
    }
}
