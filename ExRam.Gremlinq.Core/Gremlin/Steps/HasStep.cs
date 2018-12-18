using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(object key, P predicate) : base(key, predicate)
        {
        }

        public HasStep(object key, IGremlinQuery traversal) : base(key, traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
