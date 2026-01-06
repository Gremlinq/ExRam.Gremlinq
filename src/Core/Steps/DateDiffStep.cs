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
    }
}
