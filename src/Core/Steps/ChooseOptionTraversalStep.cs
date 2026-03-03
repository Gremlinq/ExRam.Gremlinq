namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the traversal used as the condition in a <c>choose()</c>/<c>option()</c> construct.</summary>
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
