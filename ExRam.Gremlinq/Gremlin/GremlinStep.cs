using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public struct GremlinStep
    {
        public GremlinStep(string name, params object[] parameters)
        {
            this.Name = name;
            this.Parameters = ImmutableList.Create(parameters);
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}