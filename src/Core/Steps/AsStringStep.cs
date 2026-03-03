namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>asString()</c> step that casts the traverser to a string type.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#string-steps">Reference Documentation - String Steps</seealso>
    public sealed class AsStringStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="AsStringStep"/>.</summary>
        public static readonly AsStringStep Instance = new ();

        private AsStringStep()
        {

        }
    }
}
