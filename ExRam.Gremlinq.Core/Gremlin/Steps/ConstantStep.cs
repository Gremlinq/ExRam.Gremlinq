using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ConstantStep : Step
    {
        public ConstantStep(object value)
        {
            Value = value;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Value { get; }
    }
}
