namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>option()</c> step modulator used within a <c>choose()</c> construct.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep(object? guard, Traversal optionTraversal) : base(optionTraversal.GetSideEffectSemanticsChange())
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        public object? Guard { get; }

        public Traversal OptionTraversal { get; }
    }
}
