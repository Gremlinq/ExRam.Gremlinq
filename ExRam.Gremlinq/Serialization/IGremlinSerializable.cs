using System.Text;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache);
    }
}