using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public abstract class Step : IGremlinQueryElement
    {
        public abstract void Accept(IGremlinQueryElementVisitor visitor);
    }
}
