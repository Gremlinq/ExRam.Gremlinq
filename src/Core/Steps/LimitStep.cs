using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>limit()</c> step that limits the number of traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#limit-step">Reference Documentation - Limit Step</seealso>
    public sealed class LimitStep : Step
    {
        /// <summary>Gets an instance that limits to 1 element in local scope.</summary>
        public static readonly LimitStep LimitLocal1 = new(1, Scope.Local);
        /// <summary>Gets an instance that limits to 1 element in global scope.</summary>
        public static readonly LimitStep LimitGlobal1 = new(1, Scope.Global);

        internal static readonly MapStep LimitLocal1Workaround = new (Traversal.Empty.Push(
            UnfoldStep.Instance,
            LimitGlobal1,
            FoldStep.Instance));

        /// <summary>Initializes a new instance of <see cref="LimitStep"/>.</summary>
        /// <param name="count">The maximum number of traversers to allow.</param>
        /// <param name="scope">The scope of the limit operation.</param>
        public LimitStep(long count, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        /// <summary>Gets the maximum number of traversers.</summary>
        public long Count { get; }
        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
