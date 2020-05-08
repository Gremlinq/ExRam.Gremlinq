namespace ExRam.Gremlinq.Core
{
    public sealed class RepeatStep : Step
    {
        public RepeatStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
