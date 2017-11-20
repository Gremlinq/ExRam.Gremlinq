using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct GremlinPredicate : IGremlinSerializable
    {
        public GremlinPredicate(object name, params object[] arguments)
        {
            this.Name = name;
            this.Arguments = arguments;
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache)
        {
            var builder = new StringBuilder();
            var dict = new Dictionary<string, object>();

            builder.Append(this.Name);
            builder.Append("(");

            for (var i = 0; i < this.Arguments.Length; i++)
            {
                var parameter = this.Arguments[i];

                if (i != 0)
                    builder.Append(", ");

                if (parameter is IGremlinSerializable serializable)
                    builder.Append(serializable.Serialize(parameterCache).queryString);
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

        public object Name { get; }
        public object[] Arguments { get; }
    }
}