namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>identity()</c> step that maps the traverser to itself.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#identity-step">Reference Documentation - Identity Step</seealso>
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new();
    }
}
