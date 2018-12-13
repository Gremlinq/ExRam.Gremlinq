using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public abstract class Step : IGremlinQueryElement
    {
        public abstract void Accept(IGremlinQueryElementVisitor visitor);
    }
}
