using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class MethodStep : NonTerminalStep
    {
        public MethodStep(string name, params object[] parameters) : this(name, ImmutableList.Create(parameters))
        {

        }

        public MethodStep(string name, IImmutableList<object> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new ResolvedMethodStep(Name, ResolveParameters(model));
        }

        private IEnumerable<object> ResolveParameters(IGraphModel model)
        {
            foreach(var parameter in Parameters)
            {
                if (parameter is IGremlinQuery subQuery)
                    yield return subQuery.Resolve(model);
                else
                    yield return parameter;
            }
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}
