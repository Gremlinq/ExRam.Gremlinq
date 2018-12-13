using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class IsStep : Step
    {
        public IsStep(object argument)
        {
            Argument = argument;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Argument { get; }
    }
}
