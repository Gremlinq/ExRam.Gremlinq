namespace ExRam.Gremlinq.Core
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(IGremlinQueryBase[] traversals) : base(traversals)
        {
        }
    }
}
