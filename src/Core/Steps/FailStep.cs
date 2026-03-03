namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>fail()</c> step that forces the traversal to fail.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#fail-step">Reference Documentation - Fail Step</seealso>
    public sealed class FailStep : Step
    {
        public static readonly FailStep NoMessage = new ();

        public FailStep(string? message = null)
        {
            Message = message;
        }

        public string? Message { get; }
    }
}
