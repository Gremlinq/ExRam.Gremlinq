namespace ExRam.Gremlinq.Core
{
    public sealed class HasNotStep : Step
    {
        public HasNotStep(Key key)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
