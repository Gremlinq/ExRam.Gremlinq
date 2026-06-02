using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides the default entry point for building Gremlin queries.
    /// </summary>
    public static class GremlinQuerySource
    {
        /// <summary>
        /// The default query source. Use this as the starting point for building Gremlin queries.
        /// Configure it with a provider (e.g. <c>g.UseGremlinServer(...)</c>) before executing queries.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly IGremlinQuerySource g = new GremlinQuery<object, object, object, IGremlinQueryBase>(
            CachingGremlinQueryEnvironment.Invalid,
            Traversal.Empty,
            ImmutableDictionary<StepLabel, LabelProjections>.Empty,
            ImmutableDictionary<object, object?>.Empty);
    }
}
