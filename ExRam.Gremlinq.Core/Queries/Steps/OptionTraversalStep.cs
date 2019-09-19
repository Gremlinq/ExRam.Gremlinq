using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep(Option<object> guard, IGremlinQuery optionTraversal)
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        public Option<object> Guard { get; }
        public IGremlinQuery OptionTraversal { get; }
    }
}
