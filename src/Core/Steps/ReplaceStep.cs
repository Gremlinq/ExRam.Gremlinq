namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>replace()</c> step that replaces occurrences of a substring.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#replace-step">Reference Documentation - Replace Step</seealso>
    public sealed class ReplaceStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ReplaceStep"/>.</summary>
        /// <param name="oldValue">The substring to find.</param>
        /// <param name="newValue">The replacement string.</param>
        public ReplaceStep(string oldValue, string newValue)
        {
            ArgumentNullException.ThrowIfNull(oldValue);
            ArgumentNullException.ThrowIfNull(newValue);

            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>Gets the substring to find.</summary>
        public string OldValue { get; }
        /// <summary>Gets the replacement string.</summary>
        public string NewValue { get; }
    }
}
