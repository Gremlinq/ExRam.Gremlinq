using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class HasNotStep : HasStepBase
    {
        public HasNotStep(object key, P predicate) : base(key, predicate)
        {
        }

        public HasNotStep(object key, IGremlinQuery traversal) : base(key, (object)traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
