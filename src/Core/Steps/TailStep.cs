using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>tail()</c> step that keeps the last traversers from the stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tail-step">Reference Documentation - Tail Step</seealso>
    public sealed class TailStep : Step
    {
        /// <summary>Gets an instance that keeps the last 1 element in local scope.</summary>
        public static readonly TailStep TailLocal1 = new(1, Scope.Local);
        /// <summary>Gets an instance that keeps the last 1 element in global scope.</summary>
        public static readonly TailStep TailGlobal1 = new(1, Scope.Global);

        internal static readonly MapStep TailLocal1Workaround = new (Traversal.Empty.Push(
            UnfoldStep.Instance,
            TailGlobal1,
            FoldStep.Instance));

        /// <summary>Initializes a new instance of <see cref="TailStep"/>.</summary>
        /// <param name="count">The number of last traversers to keep.</param>
        /// <param name="scope">The scope of the tail operation.</param>
        public TailStep(long count, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        /// <summary>Gets the number of last traversers to keep.</summary>
        public long Count { get; }
        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
