using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides administrative access to the internal structure of a Gremlin query for advanced query manipulation.
    /// </summary>
    public interface IGremlinQueryAdmin
    {
        /// <summary>
        /// Configures the query's traversal steps and optionally its projection.
        /// </summary>
        /// <typeparam name="TTargetQuery">The type of query to return.</typeparam>
        /// <param name="configurator">A function that transforms the traversal steps.</param>
        /// <param name="projectionTransformation">An optional function that transforms the projection.</param>
        /// <returns>A new query with the configured steps and projection.</returns>
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> configurator, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;
        
        /// <summary>
        /// Adds a step to the query's traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The type of query to return.</typeparam>
        /// <param name="step">The step to add.</param>
        /// <param name="projectionTransformation">An optional function that transforms the projection.</param>
        /// <returns>A new query with the added step.</returns>
        TTargetQuery AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Changes the query type without modifying the query structure.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <returns>A query of the target type.</returns>
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Configures the query's metadata by applying a transformation function.
        /// </summary>
        /// <typeparam name="TTargetQuery">The type of query to return.</typeparam>
        /// <param name="metadataTransformation">A function that transforms the metadata dictionary.</param>
        /// <returns>A new query with the transformed metadata.</returns>
        TTargetQuery ConfigureMetadata<TTargetQuery>(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Gets the query source from which this query was created.
        /// </summary>
        /// <returns>The query source.</returns>
        IGremlinQuerySource GetSource();

        /// <summary>
        /// Gets the traversal steps that compose this query.
        /// </summary>
        Traversal Steps { get; }
        
        /// <summary>
        /// Gets the query environment containing configuration and execution infrastructure.
        /// </summary>
        IGremlinQueryEnvironment Environment { get; }
        
        /// <summary>
        /// Gets the metadata dictionary containing additional query information.
        /// </summary>
        IImmutableDictionary<object, object?> Metadata { get; }
    }
}
