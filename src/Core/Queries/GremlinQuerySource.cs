using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IGremlinQuerySource g = new GremlinQuery<object, object, object, object>(
            GremlinQueryEnvironment.Invalid,
            Traversal.Empty,
            ImmutableDictionary<StepLabel, LabelProjections>.Empty,
            ImmutableDictionary<object, object?>.Empty);
    }
}
