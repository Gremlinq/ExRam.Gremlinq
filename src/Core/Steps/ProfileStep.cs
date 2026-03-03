namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>profile()</c> step that returns profiling information for the traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#profile-step">Reference Documentation - Profile Step</seealso>
    public sealed class ProfileStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="ProfileStep"/>.</summary>
        public static readonly ProfileStep Instance = new();
    }
}
