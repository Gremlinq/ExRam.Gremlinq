using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public class StringGremlinQuerySerializer<TVisitor> : IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)>
        where TVisitor : IStringGremlinQueryElementVisitor, new()
    {
        public (string queryString, IDictionary<string, object> parameters) Serialize(IGremlinQuery query)
        {
            var visitor = new TVisitor();

            visitor
                .Visit(query);

            return (visitor.GetString(), visitor.GetVariableBindings());
        }
    }
}
