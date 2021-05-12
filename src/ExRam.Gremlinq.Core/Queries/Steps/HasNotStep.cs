namespace ExRam.Gremlinq.Core
{
    public sealed class HasNotStep : Step, IIsOptimizableInWhere
    {
        public HasNotStep(Key key, QuerySemantics? semantics = default) : base(semantics)
        {
            Key = key;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new HasNotStep(Key, semantics);

        public Key Key { get; }
    }
}
