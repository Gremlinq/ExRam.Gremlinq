namespace ExRam.Gremlinq.Core
{
    // ReSharper disable once InconsistentNaming
    public abstract class SingleTraversalArgumentStep : Step
    {
        protected SingleTraversalArgumentStep(IGremlinQueryBase traversal)
        {
            Traversal = traversal;
        }

        public IGremlinQueryBase Traversal { get; }
    }
}
