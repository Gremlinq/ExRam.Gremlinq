using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class TerminalGremlinStep : GremlinStep, IGremlinSerializable
    {
        public TerminalGremlinStep(string name, params object[] parameters) : this(name, ImmutableList.Create(parameters))
        {

        }

        public TerminalGremlinStep(string name, IImmutableList<object> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache, bool inlineParameters)
        {
            var parameters = new Dictionary<string, object>();
            var builder = new StringBuilder();
            
            var appendComma = false;

            builder.Append(this.Name);
            builder.Append("(");

            foreach (var parameter in this.Parameters)
            {
                if (appendComma)
                    builder.Append(", ");
                else
                    appendComma = true;

                if (parameter is IGremlinSerializable serializable)
                {
                    var (innerQueryString, innerParameters) = serializable.Serialize(parameterCache, inlineParameters);

                    builder.Append(innerQueryString);

                    foreach (var kvp in innerParameters)
                    {
                        parameters[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    if (parameter is string && inlineParameters)
                        builder.Append($"'{parameter}'");
                    else
                    {
                        if (inlineParameters)
                            builder.Append(parameter);
                        else
                        {
                            var newParameterName = parameterCache.Cache(parameter);
                            parameters[newParameterName] = parameter;

                            builder.Append(newParameterName);
                        }
                    }
                }
            }

            builder.Append(")");
            return (builder.ToString(), parameters);
        }
        
        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}