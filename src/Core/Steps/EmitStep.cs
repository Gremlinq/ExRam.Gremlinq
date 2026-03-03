namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>emit()</c> step modulator used in repeat/until/emit loop constructs.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class EmitStep : Step
    {
        public static readonly EmitStep Instance = new();
    }
}
