using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MaxStep : Step
    {
        public static readonly MaxStep Local = new(Scope.Local);
        public static readonly MaxStep Global = new(Scope.Global);

        public MaxStep(Scope scope) : base()
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
