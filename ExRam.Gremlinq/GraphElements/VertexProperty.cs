using System.Collections.Generic;
using NullGuard;

namespace ExRam.Gremlinq.GraphElements
{
    public sealed class VertexProperty : Property, IElement
    {
        [AllowNull] public object Id { get; set; }
        [AllowNull] public string Label { get; set; }
        [AllowNull] public IDictionary<string, object> Properties { get; set; }
    }
}
