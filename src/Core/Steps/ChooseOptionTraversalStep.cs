namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the traversal used as the condition in a <c>choose()</c>/<c>option()</c> construct.</summary>
    public sealed class ChooseOptionTraversalStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ChooseOptionTraversalStep"/>.</summary>
        /// <param name="traversal">The condition traversal used in a choose/option construct.</param>
        public ChooseOptionTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the condition traversal.</summary>
        public Traversal Traversal { get; }
    }
}
