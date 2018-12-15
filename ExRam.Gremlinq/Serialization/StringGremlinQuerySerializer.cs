using System.Collections.Generic;

namespace ExRam.Gremlinq.Serialization
{
    public class StringGremlinQuerySerializer<TVisitor> : IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)>
        where TVisitor : GroovyGremlinQueryElementVisitor, new()
    {
        public (string queryString, IDictionary<string, object> parameters) Serialize(IGremlinQuery query)
        {
            var groovyBuilder = new TVisitor();

            groovyBuilder
                .Visit(query);

            return (groovyBuilder.Builder.ToString(), groovyBuilder.GetVariables());
        }
    }
}
