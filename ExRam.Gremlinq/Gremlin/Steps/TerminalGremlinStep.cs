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

        public void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache)
        {
            builder.AppendMethod(this.Name, this.Parameters, parameterCache);
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}