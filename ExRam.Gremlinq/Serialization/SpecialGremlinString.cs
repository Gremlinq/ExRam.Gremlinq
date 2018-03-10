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

        public void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(this._value);
        }
    }
}