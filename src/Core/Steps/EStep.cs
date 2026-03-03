using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>E()</c> step that reads edges by their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
    public sealed class EStep : Step
    {
        public EStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

