namespace ExRam.Gremlinq.Core
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(IGremlinQueryBase[] traversals)
        {
            Traversals = traversals;
        }

        public IGremlinQueryBase[] Traversals { get; }
    }
}
