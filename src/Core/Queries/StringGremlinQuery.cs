#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal sealed class StringGremlinQuery : GremlinQueryBase<string, object, object, IGremlinQueryBase>,
        IStringGremlinQuery
    {
        public StringGremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {

        }
    }
}
