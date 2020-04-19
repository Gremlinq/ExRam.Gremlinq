using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep(object? guard, Traversal optionTraversal)
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        public object? Guard { get; }

        public Traversal OptionTraversal { get; }
    }
}
