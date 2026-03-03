using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>range()</c> step that limits the stream to a given range.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#range-step">Reference Documentation - Range Step</seealso>
    public sealed class RangeStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="RangeStep"/>.</summary>
        /// <param name="lower">The lower bound of the range (inclusive).</param>
        /// <param name="upper">The upper bound of the range (exclusive), or -1 for unbounded.</param>
        /// <param name="scope">The scope of the range operation.</param>
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

        /// <summary>Gets the lower bound (inclusive).</summary>
        public long Lower { get; }
        /// <summary>Gets the upper bound (exclusive).</summary>
        public long Upper { get; }
        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
