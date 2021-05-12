namespace ExRam.Gremlinq.Core
{
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new MapStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
