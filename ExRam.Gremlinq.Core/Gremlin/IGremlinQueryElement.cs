using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryElement
    {
        void Accept(IGremlinQueryElementVisitor visitor);
    }
}
