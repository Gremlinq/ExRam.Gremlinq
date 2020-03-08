namespace ExRam.Gremlinq.Core
{
    public sealed class HasTraversalStep : Step
    { 
        public HasTraversalStep(object key, IGremlinQueryBase traversal)
        {
            Key = key;
            Traversal = traversal;
        }

        public object Key { get; }
        public IGremlinQueryBase Traversal { get; }
    }
}
