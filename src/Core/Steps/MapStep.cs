namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
