namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasNotStep : Step, IIsOptimizableInWhere
    {
        public HasNotStep(Key key) : base()
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
