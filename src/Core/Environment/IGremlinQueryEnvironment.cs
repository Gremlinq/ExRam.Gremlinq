using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents the complete environment for building and executing Gremlin queries,
    /// including the graph model, serializer, deserializer, executor, debugger, feature set, options, and logger.
    /// </summary>
    public interface IGremlinQueryEnvironment
    {
        /// <summary>
        /// Configures the logger by applying the specified transformation.
        /// </summary>
        /// <param name="loggerTransformation">A function that transforms the current logger.</param>
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);

        /// <summary>
        /// Configures the graph model by applying the specified transformation.
        /// </summary>
        /// <param name="modelTransformation">A function that transforms the current graph model.</param>
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);

        /// <summary>
        /// Configures the feature set by applying the specified transformation.
        /// </summary>
        /// <param name="featureSetTransformation">A function that transforms the current feature set.</param>
        IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation);

        /// <summary>
        /// Configures the serializer by applying the specified transformation.
        /// </summary>
        /// <param name="serializerTransformation">A function that transforms the current serializer.</param>
        IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation);

        /// <summary>
        /// Configures the options by applying the specified transformation.
        /// </summary>
        /// <param name="optionsTransformation">A function that transforms the current options.</param>
        IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation);

        /// <summary>
        /// Configures the deserializer by applying the specified transformation.
        /// </summary>
        /// <param name="deserializerTransformation">A function that transforms the current deserializer.</param>
        IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation);

        /// <summary>
        /// Configures the set of natively supported types by applying the specified transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current set of native types.</param>
        IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation);

        /// <summary>
        /// Configures the debugger by applying the specified transformation.
        /// </summary>
        /// <param name="debuggerTransformation">A function that transforms the current debugger.</param>
        IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation);

        /// <summary>
        /// Configures the query executor by applying the specified transformation.
        /// </summary>
        /// <param name="executorTransformation">A function that transforms the current executor.</param>
        IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);

        // The default implementation only keeps 14.x non-breaking for other implementations of this
        // interface. Remove it with the next breaking change.
        /// <summary>
        /// Configures the range of TinkerPop versions the server understands by applying the specified transformation.
        /// </summary>
        /// <param name="compatibilityTransformation">A function that transforms the current range.</param>
        IGremlinQueryEnvironment ConfigureServerCompatibilityRange(Func<VersionRange, VersionRange> compatibilityTransformation)
        {
            ArgumentNullException.ThrowIfNull(compatibilityTransformation);

            return this;
        }


        /// <summary>
        /// Gets the logger used for query diagnostics.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Gets the graph model that describes vertex and edge types.
        /// </summary>
        IGraphModel Model { get; }

        /// <summary>
        /// Gets the feature set that describes the capabilities of the target graph database.
        /// </summary>
        IFeatureSet FeatureSet { get; }

        /// <summary>
        /// Gets the transformer used for serializing queries to Gremlin bytecode.
        /// </summary>
        ITransformer Serializer { get; }

        /// <summary>
        /// Gets the configuration options for query behavior.
        /// </summary>
        IGremlinqOptions Options { get; }

        /// <summary>
        /// Gets the transformer used for deserializing query results.
        /// </summary>
        ITransformer Deserializer { get; }

        /// <summary>
        /// Gets the debugger used for producing human-readable query representations.
        /// </summary>
        IGremlinQueryDebugger Debugger { get; }

        /// <summary>
        /// Gets the executor responsible for running queries against the graph database.
        /// </summary>
        IGremlinQueryExecutor Executor { get; }

        /// <summary>
        /// Gets the set of types that are natively supported by the target graph database.
        /// </summary>
        IImmutableSet<Type> NativeTypes { get; }

        // The default implementation only keeps 14.x non-breaking for other implementations of this
        // interface. Remove it with the next breaking change.
        /// <summary>
        /// Gets the range of TinkerPop versions the server understands. Defaults to <see cref="TinkerPopVersions.V3_7_1"/> to
        /// <see cref="TinkerPopVersions.V3_7_7"/>.
        /// Where TinkerPop 3.8 changed a step, Gremlinq generates the form before 3.8 as long as the lowest version of the range is
        /// below 3.8, and the 3.8 form once it is 3.8 or higher.
        /// Likewise, a <see cref="DateTimeOffset"/> is sent as <c>gx:OffsetDateTime</c> rather than <c>g:Date</c> once the lowest version
        /// is 3.8 or higher. How a server stores and compares the two depends on the server, so check this before moving the range of a
        /// graph that already holds dates to 3.8.
        /// </summary>
        VersionRange ServerCompatibilityRange => GremlinQueryEnvironment.DefaultServerCompatibilityRange;
    }
}
