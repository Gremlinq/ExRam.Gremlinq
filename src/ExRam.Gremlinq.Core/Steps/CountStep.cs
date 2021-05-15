using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CountStep : Step
    {
        public static readonly CountStep Global = new(Scope.Global);
        public static readonly CountStep Local = new(Scope.Local);

        public CountStep(Scope scope) : base()
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
