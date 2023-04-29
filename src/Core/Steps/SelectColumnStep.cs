using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SelectColumnStep : Step
    {
        public SelectColumnStep(Column column)
        {
            Column = column;
        }

        public Column Column { get; }
    }
}
