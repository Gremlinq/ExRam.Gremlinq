using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasNotStep : Step
    {
        public HasNotStep(object key)
        {
            Key = key;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Key { get; }
    }
}
