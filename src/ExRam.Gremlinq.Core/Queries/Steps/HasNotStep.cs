namespace ExRam.Gremlinq.Core
{
    public sealed class HasNotStep : Step, IIsOptimizableInWhere
    {
        public HasNotStep(Key key, QuerySemantics? semantics = default) : base(semantics)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
