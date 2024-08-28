namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class NotStep : Step, IFilterStep
    {
        public NotStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
