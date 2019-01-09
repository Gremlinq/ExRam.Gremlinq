namespace ExRam.Gremlinq.Core
{
    // ReSharper disable once InconsistentNaming
    public abstract class SingleTraversalArgumentStep : Step
    {
        protected SingleTraversalArgumentStep(IGremlinQuery traversal)
        {
            Traversal = traversal;
        }

        public IGremlinQuery Traversal { get; }
    }
}
