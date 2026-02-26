using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
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
