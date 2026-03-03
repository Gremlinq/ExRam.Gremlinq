using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>range()</c> step that limits the stream to a given range.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#range-step">Reference Documentation - Range Step</seealso>
    public sealed class RangeStep : Step
    {
        public RangeStep(long lower, long upper, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (lower < 0)
                throw new ArgumentOutOfRangeException(nameof(lower));

            if (upper < -1)
                throw new ArgumentException(nameof(upper));

            Lower = lower;
            Upper = upper;
            Scope = scope;
        }

        public long Lower { get; }
        public long Upper { get; }
        public Scope Scope { get; }
    }
}
