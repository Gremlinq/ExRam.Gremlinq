using ExRam.Gremlinq.Core.Serialization;
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

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Option<object> Guard { get; }
        public IGremlinQuery OptionTraversal { get; }
    }
}