namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>addV()</c> step that adds a vertex to the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addvertex-step">Reference Documentation - AddVertex Step</seealso>
    public sealed class AddVStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="AddVStep"/> with the specified vertex label.</summary>
        /// <param name="label">The label of the vertex to add.</param>
        public AddVStep(string label) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(label);

            Label = label;
        }

        /// <summary>Gets the label of the vertex to add.</summary>
        public string Label { get; }
    }
}
