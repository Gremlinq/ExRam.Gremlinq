namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ConstantStep : Step
    {
        public ConstantStep(object value) : base()
        {
            Value = value;
        }

        public object Value { get; }
    }
}
