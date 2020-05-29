using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MinStep : Step
    {
        public static readonly MinStep Local = new MinStep(Scope.Local);
        public static readonly MinStep Global = new MinStep(Scope.Global);

        private MinStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
