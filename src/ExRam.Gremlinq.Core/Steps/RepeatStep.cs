namespace ExRam.Gremlinq.Core.Steps
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
