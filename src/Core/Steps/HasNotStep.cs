namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasNotStep : Step, IFilterStep
    {
        public HasNotStep(Key key)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
