using System.Collections.Generic;

namespace ExRam.Gremlinq.Serialization
{
    public interface IStringGremlinQueryElementVisitor : IGremlinQueryElementVisitor
    {
        string GetString();
        IDictionary<string, object> GetVariableBindings();
    }
}