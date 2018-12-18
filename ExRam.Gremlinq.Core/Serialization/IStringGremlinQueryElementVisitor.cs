using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IStringGremlinQueryElementVisitor : IGremlinQueryElementVisitor
    {
        string GetString();
        IDictionary<string, object> GetVariableBindings();
    }
}
