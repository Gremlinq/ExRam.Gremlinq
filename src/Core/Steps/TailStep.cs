using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>tail()</c> step that keeps the last traversers from the stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tail-step">Reference Documentation - Tail Step</seealso>
    public sealed class TailStep : Step
    {
        public static readonly TailStep TailLocal1 = new(1, Scope.Local);
        public static readonly TailStep TailGlobal1 = new(1, Scope.Global);

        internal static readonly MapStep TailLocal1Workaround = new (Traversal.Empty.Push(
            UnfoldStep.Instance,
            TailGlobal1,
            FoldStep.Instance));

        public TailStep(long count, Scope scope)
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
