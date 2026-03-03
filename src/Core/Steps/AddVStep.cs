namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>addV()</c> step that adds a vertex to the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addvertex-step">Reference Documentation - AddVertex Step</seealso>
    public sealed class AddVStep : Step
    {
        public AddVStep(string label) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(label);

            Label = label;
        }

        public string Label { get; }
    }
}
