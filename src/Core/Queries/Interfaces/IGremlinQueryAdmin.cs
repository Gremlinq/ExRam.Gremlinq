using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        /// <summary>
        /// Configures the traversal steps of this query.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type to return.</typeparam>
        /// <param name="configurator">A function that transforms the current traversal steps.</param>
        /// <param name="projectionTransformation">An optional function that transforms the projection.</param>
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> configurator, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Adds a step to the traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type to return.</typeparam>
        /// <param name="step">The step to add.</param>
        /// <param name="projectionTransformation">An optional function that transforms the projection.</param>
        TTargetQuery AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Changes the query to a different query type while preserving the traversal state.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Configures the metadata dictionary associated with this query.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type to return.</typeparam>
        /// <param name="metadataTransformation">A function that transforms the metadata dictionary.</param>
        TTargetQuery ConfigureMetadata<TTargetQuery>(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) where TTargetQuery : IStartGremlinQuery;

        /// <summary>
        /// Gets the query source that this query originated from.
        /// </summary>
        IGremlinQuerySource GetSource();

        /// <summary>
        /// Gets the current traversal steps of this query.
        /// </summary>
        Traversal Steps { get; }

        /// <summary>
        /// Gets the query environment associated with this query.
        /// </summary>
        IGremlinQueryEnvironment Environment { get; }

        /// <summary>
        /// Gets the metadata dictionary associated with this query.
        /// </summary>
        IImmutableDictionary<object, object?> Metadata { get; }
    }
}
