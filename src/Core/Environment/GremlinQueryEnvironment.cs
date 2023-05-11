using System.Collections.Immutable;
using System.Text;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core
{
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

        private sealed class TimeSpanAsNumberConverterFactory : IConverterFactory
        {
            private sealed class TimeSpanAsNumberConverter<TSource> : IConverter<TSource, TimeSpan>
            {
                private readonly IGremlinQueryEnvironment _environment;

                public TimeSpanAsNumberConverter(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public bool TryConvert(TSource source, ITransformer recurse, out TimeSpan value)
                {
                    if (recurse.TryTransformTo<double>().From(source, _environment) is { } parsedDouble)
                    {
                        value = TimeSpan.FromMilliseconds(parsedDouble);
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                return typeof(TTarget) == typeof(TimeSpan)
                    ? (IConverter<TSource, TTarget>)(object)new TimeSpanAsNumberConverter<TSource>(environment)
                    : default;
            }
        }

        public static readonly IGremlinQueryEnvironment Empty = new GremlinQueryEnvironmentImpl(
            GraphModel.Empty,
            Transformer.Identity,
            GremlinQueryExecutor.Empty,
            Transformer.Identity,
            GremlinQueryDebugger.Groovy,
            FeatureSet.Full,
            GremlinqOptions.Empty,
            ImmutableHashSet<Type>.Empty,
            NullLogger.Instance);

        public static readonly IGremlinQueryEnvironment Default = Empty
            .UseModel(GraphModel.Invalid)
            .UseSerializer(Serializer.Default)
            .UseExecutor(GremlinQueryExecutor.Invalid)
            .UseDeserializer(Deserializer.Default)
            .ConfigureNativeTypes(_ => DefaultNativeTypes);

        public static IGremlinQueryEnvironment UseModel(this IGremlinQueryEnvironment source, IGraphModel model) => source.ConfigureModel(_ => model);

        public static IGremlinQueryEnvironment UseLogger(this IGremlinQueryEnvironment source, ILogger logger) => source.ConfigureLogger(_ => logger);

        public static IGremlinQueryEnvironment UseSerializer(this IGremlinQueryEnvironment environment, ITransformer serializer) => environment.ConfigureSerializer(_ => serializer);

        public static IGremlinQueryEnvironment UseDeserializer(this IGremlinQueryEnvironment environment, ITransformer deserializer) => environment.ConfigureDeserializer(_ => deserializer);

        public static IGremlinQueryEnvironment UseExecutor(this IGremlinQueryEnvironment environment, IGremlinQueryExecutor executor) => environment.ConfigureExecutor(_ => executor);

        public static IGremlinQueryEnvironment UseDebugger(this IGremlinQueryEnvironment environment, IGremlinQueryDebugger debugger) => environment.ConfigureDebugger(_ => debugger);

        public static IGremlinQueryEnvironment UseGraphSon2(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON2Writer(), "application/vnd.gremlin-v2.0+json");

        public static IGremlinQueryEnvironment UseGraphSon3(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON3Writer(), "application/vnd.gremlin-v3.0+json");

        private static IGremlinQueryEnvironment UseGraphSon(this IGremlinQueryEnvironment environment, GraphSONWriter writer, string mimeType)
        {
            var mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}");

            return environment
                .ConfigureSerializer(serializer => serializer
                    .Add(Create<RequestMessage, byte[]>((message, _, _) =>
                    {
                        var graphSONMessage = writer.WriteObject(message);
                        var ret = new byte[Encoding.UTF8.GetByteCount(graphSONMessage) + mimeTypeBytes.Length];

                        mimeTypeBytes.CopyTo(ret, 0);
                        Encoding.UTF8.GetBytes(graphSONMessage, 0, graphSONMessage.Length, ret, mimeTypeBytes.Length);

                        return ret;
                    })));
        }
    }
}
