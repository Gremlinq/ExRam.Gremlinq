using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public abstract class Step : IGremlinQueryAtom
    {
        public abstract void Accept(IGremlinQueryElementVisitor visitor);
    }
}
