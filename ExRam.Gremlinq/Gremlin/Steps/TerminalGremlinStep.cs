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

        public void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
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
                    serializable.Serialize(builder, parameterCache);
                else
                    builder.Append(parameterCache.Cache(parameter));
            }

            builder.Append(")");
        }
        
        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}