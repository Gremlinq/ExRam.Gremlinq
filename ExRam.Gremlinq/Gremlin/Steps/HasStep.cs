using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(object key, P predicate) : base(key, predicate)
        {
        }

        public HasStep(object key, IGremlinQuery traversal) : base(key, (object)traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
