using System.Text;

namespace ExRam.Gremlinq
{
    public struct SpecialGremlinString : IGremlinSerializable
    {
        private readonly string _value;

        public SpecialGremlinString(string value)
        {
            this._value = value;
        }

        public void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache)
        {
            builder.AppendConstant(this._value, parameterCache);
        }
    }
}