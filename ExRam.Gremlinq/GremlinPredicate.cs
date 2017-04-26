using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct GremlinPredicate : IGremlinSerializable
    { 
        private readonly object _name;
        private readonly object[] _arguments;

        public GremlinPredicate(object name, params object[] arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterNameProvider parameterNameProvider, bool inlineParameters)
        {
            var builder = new StringBuilder();
            var dict = new Dictionary<string, object>();

            builder.Append((object) this._name);
            builder.Append("(");

            for (var i = 0; i < this._arguments.Length; i++)
            {
                if (i != 0)
                    builder.Append(", ");

                if (inlineParameters)
                {
                    if (this._arguments[i] is string)
                        builder.Append($"'{this._arguments[i]}'");
                    else
                        builder.Append((object) this._arguments[i]);
                }
                else
                {
                    var parameter = parameterNameProvider.Get();
                    dict[parameter] = this._arguments[i];
                    builder.Append(parameter);
                }
            }

            builder.Append(")");
            return (builder.ToString(), dict);
        }
    }
}