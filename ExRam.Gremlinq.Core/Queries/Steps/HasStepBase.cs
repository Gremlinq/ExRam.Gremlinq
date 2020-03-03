namespace ExRam.Gremlinq.Core
{
    public abstract class HasStepBase : Step
    {
        protected HasStepBase(object key, object? value)
        {
            Key = key;
            Value = value;
        }

        public object Key { get; }
        public object? Value { get; }
    }
}
