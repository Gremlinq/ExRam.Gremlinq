namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>key()</c> step that maps a property to its key.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#key-step">Reference Documentation - Key Step</seealso>
    public sealed class KeyStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="KeyStep"/>.</summary>
        public static readonly KeyStep Instance = new();
    }
}
