using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class TrimStep : Step
    {
        public static readonly TrimStep Global = new(Scope.Global);

        private TrimStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
