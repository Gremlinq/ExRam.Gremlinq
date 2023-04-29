using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MeanStep : Step
    {
        public static readonly MeanStep Local = new(Scope.Local);
        public static readonly MeanStep Global = new(Scope.Global);

        public MeanStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
