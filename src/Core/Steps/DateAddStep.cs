using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>dateAdd()</c> step that adds a duration to a date value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dateadd-step">Reference Documentation - DateAdd Step</seealso>
    public sealed class DateAddStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="DateAddStep"/>.</summary>
        /// <param name="dateToken">The date token indicating the unit of the addition (e.g. day, hour).</param>
        /// <param name="value">The amount to add.</param>
        public DateAddStep(DT dateToken, int value)
        {
            ArgumentNullException.ThrowIfNull(dateToken);

            Value = value;
            DateToken = dateToken;
        }

        /// <summary>Gets the amount to add.</summary>
        public int Value { get; }
        /// <summary>Gets the date token indicating the unit of the addition.</summary>
        public DT DateToken { get; }
    }
}
