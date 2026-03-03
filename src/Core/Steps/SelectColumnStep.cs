using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>select(column)</c> step that selects keys or values from a map.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
    public sealed class SelectColumnStep : Step
    {
        public SelectColumnStep(Column column)
        {
            ArgumentNullException.ThrowIfNull(column);

            Column = column;
        }

        public Column Column { get; }
    }
}
