using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>dateAdd()</c> step that adds a duration to a date value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dateadd-step">Reference Documentation - DateAdd Step</seealso>
    public sealed class DateAddStep : Step
    {
        public DateAddStep(DT dateToken, int value)
        {
            ArgumentNullException.ThrowIfNull(dateToken);

            Value = value;
            DateToken = dateToken;
        }

        public int Value { get; }
        public DT DateToken { get; }
    }
}
