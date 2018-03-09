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

        public string Serialize(IParameterCache parameterCache)
        {
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
                    var innerQueryString = serializable.Serialize(parameterCache);

                    builder.Append(innerQueryString);
                }
                else
                    builder.Append(parameterCache.Cache(parameter));
            }

            builder.Append(")");
            return builder.ToString();
        }
        
        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}