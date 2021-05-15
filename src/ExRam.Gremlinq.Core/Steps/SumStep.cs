using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SumStep : Step
    {
        public static readonly SumStep Local = new(Scope.Local);
        public static readonly SumStep Global = new(Scope.Global);

        public SumStep(Scope scope) : base()
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
