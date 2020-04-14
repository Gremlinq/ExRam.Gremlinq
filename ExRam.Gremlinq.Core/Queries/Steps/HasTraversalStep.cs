namespace ExRam.Gremlinq.Core
{
    public sealed class HasTraversalStep : Step
    { 
        public HasTraversalStep(object key, Traversal traversal)
        {
            Key = key;
            Traversal = traversal;
        }

        public object Key { get; }
        public Traversal Traversal { get; }
    }
}
