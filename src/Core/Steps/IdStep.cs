namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>id()</c> step that maps elements to their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#id-step">Reference Documentation - Id Step</seealso>
    public sealed class IdStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="IdStep"/>.</summary>
        public static readonly IdStep Instance = new();
    }
}
