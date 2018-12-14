using System.Collections.Generic;

namespace ExRam.Gremlinq.Serialization
{
    public class GroovyGremlinQuerySerializer : IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)>
    {
        public (string queryString, IDictionary<string, object> parameters) Serialize(IGremlinQuery query)
        {
            var groovyBuilder = new GroovyGremlinQueryElementVisitor();

            groovyBuilder
                .Visit(query);

            return (groovyBuilder.Builder.ToString(), groovyBuilder.GetVariables());
        }
    }
}
