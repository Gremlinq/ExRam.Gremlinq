using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironment
    {
        private sealed class GremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public GremlinQueryEnvironmentImpl(
                IGraphModel model,
                ISerializer serializer,
                IGremlinQueryExecutor executor,
                ITransformer deserializer,
                IGremlinQueryDebugger debugger,
                IFeatureSet featureSet,
                IGremlinqOptions options,
                ILogger logger)
            {
                Model = model;
                Logger = logger;
                Options = options;
                Executor = executor;
                Debugger = debugger;
                FeatureSet = featureSet;
                Serializer = serializer;
                Deserializer = deserializer;
            }

            public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => new GremlinQueryEnvironmentImpl(modelTransformation(Model), Serializer, Executor, Deserializer, Debugger, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, FeatureSet, optionsTransformation(Options), Logger);

            public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, featureSetTransformation(FeatureSet), Options, Logger);

            public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, FeatureSet, Options, loggerTransformation(Logger));

            public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> configurator) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, configurator(Deserializer), Debugger, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureSerializer(Func<ISerializer, ISerializer> configurator) => new GremlinQueryEnvironmentImpl(Model, configurator(Serializer), Executor, Deserializer, Debugger, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> configurator) => new GremlinQueryEnvironmentImpl(Model, Serializer, configurator(Executor), Deserializer, Debugger, FeatureSet, Options, Logger);

            public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, debuggerTransformation(Debugger), FeatureSet, Options, Logger);

            public ILogger Logger { get; }
            public IGraphModel Model { get; }
            public IFeatureSet FeatureSet { get; }
            public IGremlinqOptions Options { get; }
            public IGremlinQueryDebugger Debugger { get; }
            public IGremlinQueryExecutor Executor { get; }
            public ISerializer Serializer { get; }
            public ITransformer Deserializer { get; }
        }

        public static readonly IGremlinQueryEnvironment Empty = new GremlinQueryEnvironmentImpl(
            GraphModel.Empty,
            Serializer.Identity,
            GremlinQueryExecutor.Empty,
            Deserializer.Identity,
            GremlinQueryDebugger.Groovy,
            FeatureSet.Full,
            GremlinqOptions.Empty,
            NullLogger.Instance);

        public static readonly IGremlinQueryEnvironment Default = Empty
            .UseSerializer(Serializer.Default)
            .UseExecutor(GremlinQueryExecutor.Invalid)
            .UseDeserializer(Deserializer.Default);

        public static IGremlinQueryEnvironment UseModel(this IGremlinQueryEnvironment source, IGraphModel model) => source.ConfigureModel(_ => model);

        public static IGremlinQueryEnvironment UseLogger(this IGremlinQueryEnvironment source, ILogger logger) => source.ConfigureLogger(_ => logger);

        public static IGremlinQueryEnvironment UseSerializer(this IGremlinQueryEnvironment environment, ISerializer serializer) => environment.ConfigureSerializer(_ => serializer);

        public static IGremlinQueryEnvironment UseDeserializer(this IGremlinQueryEnvironment environment, ITransformer deserializer) => environment.ConfigureDeserializer(_ => deserializer);

        public static IGremlinQueryEnvironment UseExecutor(this IGremlinQueryEnvironment environment, IGremlinQueryExecutor executor) => environment.ConfigureExecutor(_ => executor);

        public static IGremlinQueryEnvironment UseDebugger(this IGremlinQueryEnvironment environment, IGremlinQueryDebugger debugger) => environment.ConfigureDebugger(_ => debugger);

        public static IGremlinQueryEnvironment StoreByteArraysAsBase64String(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureModel(static model => model
                    .ConfigureNativeTypes(static types => types
                        .Remove(typeof(byte[]))))
                .ConfigureSerializer(static _ => _
                    .Override<byte[]>(static (bytes, env, recurse) => recurse.Serialize(Convert.ToBase64String(bytes), env)));
        }

        public static IGremlinQueryEnvironment RegisterNativeType<TNative>(this IGremlinQueryEnvironment environment, GremlinQueryFragmentSerializerDelegate<TNative> serializerDelegate, Func<ITransformer, ITransformer> deserializerTransformation)
        {
            return environment
                .ConfigureModel(static _ => _
                    .ConfigureNativeTypes(static _ => _
                        .Add(typeof(TNative))))
                .ConfigureSerializer(_ => _
                    .Override(serializerDelegate))
                .ConfigureDeserializer(deserializerTransformation);
        }
    }
}
