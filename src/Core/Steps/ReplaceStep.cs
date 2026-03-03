namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>replace()</c> step that replaces occurrences of a substring.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#replace-step">Reference Documentation - Replace Step</seealso>
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
