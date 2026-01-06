namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class DateDiffStep : Step
    {
        public sealed class Constant : DateDiffStep
        {
            public Constant(DateTimeOffset value)
            {
                Value = value;
            }

            public DateTimeOffset Value { get; }
        }

        public sealed class Traversal : DateDiffStep
        {
            public Traversal(Core.Traversal valueTraversal)
            {
                ValueTraversal = valueTraversal;
            }

            public Core.Traversal ValueTraversal { get; }
        }
    }
}
