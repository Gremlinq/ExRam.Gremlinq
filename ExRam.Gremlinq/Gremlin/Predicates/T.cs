using System.Text;

namespace ExRam.Gremlinq
{
    internal struct T : IGremlinSerializable
    {
        public static readonly T Id = new T("id");

        private readonly object _name;

        public T(object name)
        {
            this._name = name;
        }

        public void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(nameof(T));
            builder.Append(".");
            builder.Append(this._name);
        }
    }
}