namespace ExRam.Gremlinq.Core
{
    public sealed class ConstantStep : Step
    {
        public ConstantStep(object value)
        {
            Value = value;
        }

        public object Value { get; }
    }
}
