using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SubstringStep : Step
    {
        public SubstringStep(Range range, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Range = range;
            Scope = scope;
        }

        public Range Range { get; }
        public Scope Scope { get; }
    }
}
