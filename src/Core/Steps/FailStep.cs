namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>fail()</c> step that forces the traversal to fail.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#fail-step">Reference Documentation - Fail Step</seealso>
    public sealed class FailStep : Step
    {
        /// <summary>Gets an instance without a failure message.</summary>
        public static readonly FailStep NoMessage = new ();

        /// <summary>Initializes a new instance of <see cref="FailStep"/> with an optional message.</summary>
        /// <param name="message">An optional failure message.</param>
        public FailStep(string? message = null)
        {
            Message = message;
        }

        /// <summary>Gets the optional failure message.</summary>
        public string? Message { get; }
    }
}
