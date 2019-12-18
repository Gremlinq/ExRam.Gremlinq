using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(IGremlinQueryBase thenTraversal, Option<IGremlinQueryBase> elseTraversal = default)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public IGremlinQueryBase ThenTraversal { get; }
        public Option<IGremlinQueryBase> ElseTraversal { get; }
    }
}
