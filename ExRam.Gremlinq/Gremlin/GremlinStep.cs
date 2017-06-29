using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public struct GremlinStep
    {
        public GremlinStep(string name, params object[] parameters) : this(name, ImmutableList.Create(parameters))
        {

        }

        public GremlinStep(string name, IImmutableList<object> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }
        
        public IEnumerable<GremlinStep> Resolve(IGraphModel model)
        {
            yield return this;
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}