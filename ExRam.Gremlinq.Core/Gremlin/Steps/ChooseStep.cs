using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseStep : Step
    {
        public ChooseStep(IGremlinQuery ifTraversal, IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default)
        {
            IfTraversal = ifTraversal;
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IGremlinQuery IfTraversal { get; }
        public IGremlinQuery ThenTraversal { get; }
        public Option<IGremlinQuery> ElseTraversal { get; }
    }
}
