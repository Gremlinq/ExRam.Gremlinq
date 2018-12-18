namespace ExRam.Gremlinq.Core
{
    public abstract class SingleTraversalArgumentStep : Step
    {
        protected SingleTraversalArgumentStep(IGremlinQuery traversal)
        {
            Traversal = traversal;
        }

        public IGremlinQuery Traversal { get; }
    }
}
