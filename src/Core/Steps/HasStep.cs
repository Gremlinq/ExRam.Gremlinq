namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasStep : Step, IIsOptimizableInWhere
    {
        public HasStep(Key key)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
