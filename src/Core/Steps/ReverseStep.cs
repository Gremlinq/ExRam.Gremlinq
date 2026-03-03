namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>reverse()</c> step that reverses a string or list.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#reverse-step">Reference Documentation - Reverse Step</seealso>
    public sealed class ReverseStep : Step
    {
        public static readonly ReverseStep Instance = new ();

        private ReverseStep()
        {

        }
    }
}
