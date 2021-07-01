namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class Step
    {
        protected Step(TraversalSemanticsChange traversalSemanticsChange = TraversalSemanticsChange.None)
        {
            TraversalSemanticsChange = traversalSemanticsChange;
        }

        public TraversalSemanticsChange TraversalSemanticsChange { get; }
    }
}
