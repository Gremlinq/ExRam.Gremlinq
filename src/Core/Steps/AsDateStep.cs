namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>asDate()</c> step that casts the traverser to a date type.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datetime-steps">Reference Documentation - DateTime Steps</seealso>
    public sealed class AsDateStep : Step
    {
        public static readonly AsDateStep Instance = new();

        private AsDateStep()
        {

        }
    }
}
