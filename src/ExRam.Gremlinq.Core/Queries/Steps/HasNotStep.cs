namespace ExRam.Gremlinq.Core
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
