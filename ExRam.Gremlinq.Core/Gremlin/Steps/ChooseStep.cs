using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public IGremlinQuery ThenTraversal { get; }
        public Option<IGremlinQuery> ElseTraversal { get; }
    }
}
