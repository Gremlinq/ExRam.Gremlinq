namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>label()</c> step that maps an element to its label.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#label-step">Reference Documentation - Label Step</seealso>
    public sealed class LabelStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="LabelStep"/>.</summary>
        public static readonly LabelStep Instance = new();
    }
}
