namespace ExRam.Gremlinq.Core
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(Traversal[] traversals)
        {
            Traversals = traversals;
        }

        public Traversal[] Traversals { get; }
    }
}
