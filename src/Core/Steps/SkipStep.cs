using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>skip()</c> step that skips a number of traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#skip-step">Reference Documentation - Skip Step</seealso>
    public sealed class SkipStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="SkipStep"/>.</summary>
        /// <param name="count">The number of traversers to skip.</param>
        /// <param name="scope">The scope of the skip operation.</param>
        public SkipStep(long count, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        /// <summary>Gets the number of traversers to skip.</summary>
        public long Count { get; }
        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
