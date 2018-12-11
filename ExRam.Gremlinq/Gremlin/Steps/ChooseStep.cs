using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class ChooseStep : Step
    {
        public ChooseStep(IGremlinQuery ifTraversal, IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default)
        {
            IfTraversal = ifTraversal;
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IGremlinQuery IfTraversal { get; }
        public IGremlinQuery ThenTraversal { get; }
        public Option<IGremlinQuery> ElseTraversal { get; }
    }
}
