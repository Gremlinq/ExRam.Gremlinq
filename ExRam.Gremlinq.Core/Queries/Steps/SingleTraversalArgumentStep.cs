namespace ExRam.Gremlinq.Core
{
    // ReSharper disable once InconsistentNaming
    public abstract class SingleTraversalArgumentStep : Step
    {
        protected SingleTraversalArgumentStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
