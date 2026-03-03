namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for the Gremlin <c>dateDiff()</c> step that computes the difference between dates.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datediff-step">Reference Documentation - DateDiff Step</seealso>
    public abstract class DateDiffStep : Step
    {
        /// <summary>Represents a <c>dateDiff()</c> step with a constant date value.</summary>
        public sealed class Constant : DateDiffStep
        {
            /// <summary>Initializes a new instance of <see cref="Constant"/>.</summary>
            /// <param name="value">The constant date value to compare against.</param>
            public Constant(DateTimeOffset value)
            {
                Value = value;
            }

            /// <summary>Gets the constant date value.</summary>
            public DateTimeOffset Value { get; }
        }

        /// <summary>Represents a <c>dateDiff()</c> step with a traversal-based date value.</summary>
        public sealed class Traversal : DateDiffStep
        {
            /// <summary>Initializes a new instance of <see cref="Traversal"/>.</summary>
            /// <param name="valueTraversal">The traversal producing the date value to compare against.</param>
            public Traversal(Core.Traversal valueTraversal)
            {
                ValueTraversal = valueTraversal;
            }

            /// <summary>Gets the traversal producing the date value.</summary>
            public Core.Traversal ValueTraversal { get; }
        }
    }
}
