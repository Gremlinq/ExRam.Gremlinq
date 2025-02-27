using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class TrimEndStep : Step
    {
        public static readonly TrimEndStep Global = new(Scope.Global);

        private TrimEndStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
