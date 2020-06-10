namespace ExRam.Gremlinq.Core
{
    public sealed class HasTraversalStep : Step
    { 
        public HasTraversalStep(Key key, Traversal traversal)
        {
            Key = key;
            Traversal = traversal;
        }

        public Key Key { get; }
        public Traversal Traversal { get; }
    }
}
