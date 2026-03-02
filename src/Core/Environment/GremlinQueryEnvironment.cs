using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides the default <see cref="IGremlinQueryEnvironment"/> instance and extension methods for configuring environments.
    /// </summary>
    public static class GremlinQueryEnvironment
    {
        private static readonly IImmutableSet<Type> DefaultNativeTypes = new[]
            {
                typeof(bool),
                typeof(byte),
                typeof(byte[]),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(string),
                typeof(Guid),
                typeof(TimeSpan),
                typeof(DateTime),
                typeof(DateTimeOffset)
            }.ToImmutableHashSet();

        private sealed class GremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public GremlinQueryEnvironmentImpl(
                IGraphModel model,
                ITransformer serializer,
                IGremlinQueryExecutor executor,
                ITransformer deserializer,
                IGremlinQueryDebugger debugger,
                IFeatureSet featureSet,
                IGremlinqOptions options,
                IImmutableSet<Type> nativeTypes,
                ILogger logger)
            {
                Model = model;
                Logger = logger;
                Options = options;
                Executor = executor;
                Debugger = debugger;
                FeatureSet = featureSet;
                Serializer = serializer;
                NativeTypes = nativeTypes;
                Deserializer = deserializer;
            }

            public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => new GremlinQueryEnvironmentImpl(modelTransformation(Model), Serializer, Executor, Deserializer, Debugger, FeatureSet, Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, FeatureSet, optionsTransformation(Options), NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, featureSetTransformation(FeatureSet), Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, FeatureSet, Options, NativeTypes, loggerTransformation(Logger));

            public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> configurator) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, configurator(Deserializer), Debugger, FeatureSet, Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> configurator) => new GremlinQueryEnvironmentImpl(Model, configurator(Serializer), Executor, Deserializer, Debugger, FeatureSet, Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> configurator) => new GremlinQueryEnvironmentImpl(Model, Serializer, configurator(Executor), Deserializer, Debugger, FeatureSet, Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, debuggerTransformation(Debugger), FeatureSet, Options, NativeTypes, Logger);

            public IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation) => new GremlinQueryEnvironmentImpl(Model, Serializer, Executor, Deserializer, Debugger, FeatureSet, Options, transformation(NativeTypes), Logger);


            public ILogger Logger { get; }
            public IGraphModel Model { get; }
            public IFeatureSet FeatureSet { get; }
            public ITransformer Serializer { get; }
            public IGremlinqOptions Options { get; }
            public ITransformer Deserializer { get; }
            public IGremlinQueryDebugger Debugger { get; }
            public IGremlinQueryExecutor Executor { get; }
            public IImmutableSet<Type> NativeTypes { get; }
        }

        /// <summary>
        /// The default environment that has no provider configured. Executing queries on this environment will throw.
        /// </summary>
        public static readonly IGremlinQueryEnvironment Invalid = new GremlinQueryEnvironmentImpl(
            GraphModel.Invalid,
            Serializer.Default,
            GremlinQueryExecutor.Invalid,
            Deserializer.Default,
            GremlinQueryDebugger.Groovy,
            FeatureSet.Full,
            GremlinqOptions.Empty,
            DefaultNativeTypes,
            NullLogger.Instance);

        /// <summary>
        /// Replaces the graph model on the environment.
        /// </summary>
        /// <param name="source">The environment to configure.</param>
        /// <param name="model">The graph model to use.</param>
        public static IGremlinQueryEnvironment UseModel(this IGremlinQueryEnvironment source, IGraphModel model)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(model);

            return source.ConfigureModel(_ => model);
        }

        /// <summary>
        /// Replaces the logger on the environment.
        /// </summary>
        /// <param name="source">The environment to configure.</param>
        /// <param name="logger">The logger to use.</param>
        public static IGremlinQueryEnvironment UseLogger(this IGremlinQueryEnvironment source, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(logger);

            return source.ConfigureLogger(_ => logger);
        }

        /// <summary>
        /// Replaces the serializer on the environment.
        /// </summary>
        /// <param name="environment">The environment to configure.</param>
        /// <param name="serializer">The serializer to use.</param>
        public static IGremlinQueryEnvironment UseSerializer(this IGremlinQueryEnvironment environment, ITransformer serializer)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(serializer);

            return environment.ConfigureSerializer(_ => serializer);
        }

        /// <summary>
        /// Replaces the deserializer on the environment.
        /// </summary>
        /// <param name="environment">The environment to configure.</param>
        /// <param name="deserializer">The deserializer to use.</param>
        public static IGremlinQueryEnvironment UseDeserializer(this IGremlinQueryEnvironment environment, ITransformer deserializer)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(deserializer);

            return environment.ConfigureDeserializer(_ => deserializer);
        }

        /// <summary>
        /// Replaces the query executor on the environment.
        /// </summary>
        /// <param name="environment">The environment to configure.</param>
        /// <param name="executor">The executor to use.</param>
        public static IGremlinQueryEnvironment UseExecutor(this IGremlinQueryEnvironment environment, IGremlinQueryExecutor executor)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(executor);

            return environment.ConfigureExecutor(_ => executor);
        }

        /// <summary>
        /// Replaces the debugger on the environment.
        /// </summary>
        /// <param name="environment">The environment to configure.</param>
        /// <param name="debugger">The debugger to use.</param>
        public static IGremlinQueryEnvironment UseDebugger(this IGremlinQueryEnvironment environment, IGremlinQueryDebugger debugger)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(debugger);

            return environment.ConfigureDebugger(_ => debugger);
        }
    }
}
