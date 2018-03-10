using System.Text;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        void Serialize(StringBuilder builder, IParameterCache parameterCache);
    }
}