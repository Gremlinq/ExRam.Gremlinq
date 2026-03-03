namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for the Gremlin <c>dateDiff()</c> step that computes the difference between dates.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datediff-step">Reference Documentation - DateDiff Step</seealso>
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
