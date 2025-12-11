using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents the execution environment for Gremlin queries, providing access to configuration and query execution infrastructure.
    /// </summary>
    public interface IGremlinQueryEnvironment
    {
        /// <summary>
        /// Configures the logger by applying a transformation function.
        /// </summary>
        /// <param name="loggerTransformation">A function that transforms the logger.</param>
        /// <returns>A new environment with the transformed logger.</returns>
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);
        
        /// <summary>
        /// Configures the graph model by applying a transformation function.
        /// </summary>
        /// <param name="modelTransformation">A function that transforms the graph model.</param>
        /// <returns>A new environment with the transformed model.</returns>
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        
        /// <summary>
        /// Configures the feature set by applying a transformation function.
        /// </summary>
        /// <param name="featureSetTransformation">A function that transforms the feature set.</param>
        /// <returns>A new environment with the transformed feature set.</returns>
        IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation);
        
        /// <summary>
        /// Configures the query serializer by applying a transformation function.
        /// </summary>
        /// <param name="serializerTransformation">A function that transforms the serializer.</param>
        /// <returns>A new environment with the transformed serializer.</returns>
        IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation);
        
        /// <summary>
        /// Configures the query options by applying a transformation function.
        /// </summary>
        /// <param name="optionsTransformation">A function that transforms the options.</param>
        /// <returns>A new environment with the transformed options.</returns>
        IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation);
        
        /// <summary>
        /// Configures the result deserializer by applying a transformation function.
        /// </summary>
        /// <param name="deserializerTransformation">A function that transforms the deserializer.</param>
        /// <returns>A new environment with the transformed deserializer.</returns>
        IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation);
        
        /// <summary>
        /// Configures the set of native types by applying a transformation function.
        /// </summary>
        /// <param name="transformation">A function that transforms the native types set.</param>
        /// <returns>A new environment with the transformed native types.</returns>
        IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation);
        
        /// <summary>
        /// Configures the query debugger by applying a transformation function.
        /// </summary>
        /// <param name="debuggerTransformation">A function that transforms the debugger.</param>
        /// <returns>A new environment with the transformed debugger.</returns>
        IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation);
        
        /// <summary>
        /// Configures the query executor by applying a transformation function.
        /// </summary>
        /// <param name="executorTransformation">A function that transforms the executor.</param>
        /// <returns>A new environment with the transformed executor.</returns>
        IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);


        /// <summary>
        /// Gets the logger used for query execution logging.
        /// </summary>
        ILogger Logger { get; }
        
        /// <summary>
        /// Gets the graph model that defines the structure of vertices and edges.
        /// </summary>
        IGraphModel Model { get; }
        
        /// <summary>
        /// Gets the feature set that defines the capabilities of the target database.
        /// </summary>
        IFeatureSet FeatureSet { get; }
        
        /// <summary>
        /// Gets the transformer used to serialize queries.
        /// </summary>
        ITransformer Serializer { get; }
        
        /// <summary>
        /// Gets the query options.
        /// </summary>
        IGremlinqOptions Options { get; }
        
        /// <summary>
        /// Gets the transformer used to deserialize query results.
        /// </summary>
        ITransformer Deserializer { get; }
        
        /// <summary>
        /// Gets the debugger for query inspection and debugging.
        /// </summary>
        IGremlinQueryDebugger Debugger { get; }
        
        /// <summary>
        /// Gets the executor responsible for executing queries against the database.
        /// </summary>
        IGremlinQueryExecutor Executor { get; }
        
        /// <summary>
        /// Gets the set of types that are considered native by the target database and don't require serialization.
        /// </summary>
        IImmutableSet<Type> NativeTypes { get; }
    }
}
