namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>option()</c> step modulator used within a <c>choose()</c> construct.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
    public sealed class OptionTraversalStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="OptionTraversalStep"/>.</summary>
        /// <param name="guard">The guard value that selects this option branch, or <see langword="null"/> for the default branch.</param>
        /// <param name="optionTraversal">The traversal to execute for this option.</param>
        public OptionTraversalStep(object? guard, Traversal optionTraversal) : base(optionTraversal.GetSideEffectSemanticsChange())
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        /// <summary>Gets the guard value, or <see langword="null"/> for the default branch.</summary>
        public object? Guard { get; }

        /// <summary>Gets the option traversal.</summary>
        public Traversal OptionTraversal { get; }
    }
}
