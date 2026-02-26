namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ReplaceStep : Step
    {
        public ReplaceStep(string oldValue, string newValue)
        {
            ArgumentNullException.ThrowIfNull(oldValue);
            ArgumentNullException.ThrowIfNull(newValue);

            OldValue = oldValue;
            NewValue = newValue;
        }

        public string OldValue { get; }
        public string NewValue { get; }
    }
}
