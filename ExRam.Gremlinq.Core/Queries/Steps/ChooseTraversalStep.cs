using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(IGremlinQueryBase ifTraversal, IGremlinQueryBase thenTraversal, Option<IGremlinQueryBase> elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            IfTraversal = ifTraversal;
        }

        public IGremlinQueryBase IfTraversal { get; }
    }
}
