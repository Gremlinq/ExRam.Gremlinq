namespace ExRam.Gremlinq.Core
{
    public sealed class RepeatStep : Step
    {
        public RepeatStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
