namespace ExRam.Gremlinq.Core
{
    public sealed class HasNotStep : Step
    {
        public HasNotStep(object key)
        {
            Key = key;
        }

        public object Key { get; }
    }
}
