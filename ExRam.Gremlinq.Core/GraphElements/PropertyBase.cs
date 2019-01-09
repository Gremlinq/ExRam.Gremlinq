using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public abstract class PropertyBase
    {
        internal abstract object GetValue();
        internal abstract IDictionary<string, object> GetMetaProperties();
    }
}
