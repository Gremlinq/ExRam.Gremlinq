using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MeanStep : Step
    {
        public static readonly MeanStep Local = new MeanStep(Scope.Local);
        public static readonly MeanStep Global = new MeanStep(Scope.Global);

        private MeanStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
