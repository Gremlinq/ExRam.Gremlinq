using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ToUpperStep : Step
    {
        public static readonly ToUpperStep Global = new(Scope.Global);

        private ToUpperStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
