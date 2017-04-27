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

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache, bool inlineParameters)
        {
            var builder = new StringBuilder();
            var dict = new Dictionary<string, object>();

            builder.Append((object) this._name);
            builder.Append("(");

            for (var i = 0; i < this._arguments.Length; i++)
            {
                var parameter = this._arguments[i];

                if (i != 0)
                    builder.Append(", ");

                if (inlineParameters)
                {
                    if (parameter is string)
                        builder.Append($"'{parameter}'");
                    else
                        builder.Append(parameter);
                }
                else
                {
                    var parameterName = parameterCache.Cache(parameter);
                    dict[parameterName] = parameter;
                    builder.Append(parameterName);
                }
            }

            builder.Append(")");
            return (builder.ToString(), dict);
        }
    }
}