using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep([AllowNull] object? guard, IGremlinQueryBase optionTraversal)
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        [AllowNull] public object? Guard { get; }
        public IGremlinQueryBase OptionTraversal { get; }
    }
}
