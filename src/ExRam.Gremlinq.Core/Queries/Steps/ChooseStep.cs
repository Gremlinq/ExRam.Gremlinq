namespace ExRam.Gremlinq.Core
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(Traversal thenTraversal, Traversal? elseTraversal = default, QuerySemantics? semantics = default) : base(semantics)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public Traversal ThenTraversal { get; }

        public Traversal? ElseTraversal { get; }
    }
}
