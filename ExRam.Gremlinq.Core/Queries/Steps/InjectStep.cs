using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class InjectStep : Step
    {
        public object[] Elements { get; }

        public InjectStep(object[] elements)
        {
            Elements = elements;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
