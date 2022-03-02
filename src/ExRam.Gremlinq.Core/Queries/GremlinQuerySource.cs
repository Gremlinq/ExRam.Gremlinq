using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IConfigurableGremlinQuerySource g = new GremlinQuery<object, object, object, object, object, object>(
            StepStack.Empty,
            Projection.Empty,
            GremlinQueryEnvironment.Default,
            ImmutableDictionary<StepLabel, Projection>.Empty,
            QueryFlags.SurfaceVisible);
    }
}
