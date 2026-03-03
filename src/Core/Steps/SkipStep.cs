using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>skip()</c> step that skips a number of traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#skip-step">Reference Documentation - Skip Step</seealso>
    public sealed class SkipStep : Step
    {
        public SkipStep(long count, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        public long Count { get; }
        public Scope Scope { get; }
    }
}
