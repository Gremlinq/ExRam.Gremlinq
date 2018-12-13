using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryElement
    {
        void Accept(IGremlinQueryElementVisitor visitor);
    }
}
