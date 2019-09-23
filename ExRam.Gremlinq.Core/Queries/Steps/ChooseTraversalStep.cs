using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(IGremlinQuery ifTraversal, IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            IfTraversal = ifTraversal;
        }

        public IGremlinQuery IfTraversal { get; }
    }
}
