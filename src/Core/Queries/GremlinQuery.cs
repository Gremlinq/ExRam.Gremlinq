#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal sealed partial class GremlinQuery<T1, T2, T3, T4> : GremlinQueryBase<T1, T2, T3, T4>
        where T4 : IGremlinQueryBase
    {
        public GremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {

        }
    }
}
