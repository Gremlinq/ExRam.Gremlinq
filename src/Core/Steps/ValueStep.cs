namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>value()</c> step that maps a property to its value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#value-step">Reference Documentation - Value Step</seealso>
    public sealed class ValueStep : Step
    {
        public static readonly ValueStep Instance = new();
    }
}
