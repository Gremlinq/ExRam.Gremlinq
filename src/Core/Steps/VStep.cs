using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>V()</c> step that reads vertices by their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
    public sealed class VStep : Step
    {
        public VStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

