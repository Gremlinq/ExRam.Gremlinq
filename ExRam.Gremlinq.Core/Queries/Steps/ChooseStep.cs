namespace ExRam.Gremlinq.Core
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(IGremlinQueryBase thenTraversal, IGremlinQueryBase? elseTraversal = default)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public IGremlinQueryBase ThenTraversal { get; }
        public IGremlinQueryBase? ElseTraversal { get; }
    }
}
