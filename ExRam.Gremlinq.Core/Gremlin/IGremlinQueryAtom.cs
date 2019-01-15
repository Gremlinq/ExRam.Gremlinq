using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAtom
    {
        void Accept(IGremlinQueryElementVisitor visitor);
    }
}
