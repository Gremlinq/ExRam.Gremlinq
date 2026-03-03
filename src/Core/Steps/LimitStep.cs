using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>limit()</c> step that limits the number of traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#limit-step">Reference Documentation - Limit Step</seealso>
    public sealed class LimitStep : Step
    {
        public static readonly LimitStep LimitLocal1 = new(1, Scope.Local);
        public static readonly LimitStep LimitGlobal1 = new(1, Scope.Global);

        internal static readonly MapStep LimitLocal1Workaround = new (Traversal.Empty.Push(
            UnfoldStep.Instance,
            LimitGlobal1,
            FoldStep.Instance));

        public LimitStep(long count, Scope scope)
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
