namespace ExRam.Gremlinq.Core
{
    public sealed class NotStep : Step, IIsOptimizableInWhere
    {
        public NotStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
