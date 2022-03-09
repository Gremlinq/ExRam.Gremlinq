using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IConfigurableGremlinQuerySource g = new GremlinQuery<object, object, object, object, object, object>(
            GremlinQueryEnvironment.Default,
            StepStack.Empty,
            Projection.Empty,
            ImmutableDictionary<StepLabel, Projection>.Empty,
            ImmutableDictionary<StepLabel, Projection>.Empty,
            QueryFlags.SurfaceVisible);
    }
}
