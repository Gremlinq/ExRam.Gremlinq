namespace ExRam.Gremlinq.Core
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(IGremlinQueryBase[] traversals) : base(traversals)
        {
        }
    }
}
