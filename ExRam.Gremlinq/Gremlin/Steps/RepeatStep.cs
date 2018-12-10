namespace ExRam.Gremlinq
{
    public sealed class RepeatStep : SingleTraversalArgumentStep
    {
        public RepeatStep(IGremlinQuery traversal) : base("repeat", traversal)
        {
        }
    }
}