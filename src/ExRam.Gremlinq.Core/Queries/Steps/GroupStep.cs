namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(QuerySemantics? semantics = default) : base(semantics)
            {
            }
        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key, QuerySemantics? semantics = default) : base(semantics)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public static readonly GroupStep Instance = new();

        public GroupStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }
    }
}
