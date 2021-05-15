using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MinStep : Step
    {
        public static readonly MinStep Local = new(Scope.Local);
        public static readonly MinStep Global = new(Scope.Global);

        public MinStep(Scope scope) : base()
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
