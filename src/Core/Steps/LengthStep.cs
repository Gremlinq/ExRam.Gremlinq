using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class LengthStep : Step
    {
        public static readonly LengthStep Global = new(Scope.Global);

        private LengthStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
