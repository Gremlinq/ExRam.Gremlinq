namespace ExRam.Gremlinq.Core
{
    public sealed class ConstantStep : Step
    {
        public ConstantStep(object value, QuerySemantics? semantics = default) : base(semantics)
        {
            Value = value;
        }

        public object Value { get; }
    }
}
