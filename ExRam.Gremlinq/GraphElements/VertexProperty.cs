using System.Collections.Generic;
using NullGuard;

namespace ExRam.Gremlinq.GraphElements
{
    public sealed class VertexProperty : Element
    {
        [AllowNull] public object Value { get; set; }
        [AllowNull] public IDictionary<string, object> Properties { get; set; }
    }
}
