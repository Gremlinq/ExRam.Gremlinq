using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class IsStep : Step, IFilterStep
    {
        public IsStep(P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
