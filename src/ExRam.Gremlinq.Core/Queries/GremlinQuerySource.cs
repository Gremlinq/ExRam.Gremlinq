using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IConfigurableGremlinQuerySource g = new GremlinQuery<object, object, object, object, object, object>(
            GremlinQueryEnvironment.Default,
            Traversal.Empty,
            ImmutableDictionary<StepLabel, LabelProjections>.Empty,
            QueryFlags.SurfaceVisible);
    }
}
