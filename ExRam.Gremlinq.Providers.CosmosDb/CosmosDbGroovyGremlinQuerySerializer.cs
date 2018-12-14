using System.Collections.Generic;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public class CosmosDbGroovyGremlinQuerySerializer : IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)>
    {
        private sealed class IntIdToStringIdGroovyGremlinQueryElementVisitor : GroovyGremlinQueryElementVisitor
        {
            public override void Visit(PropertyStep step)
            {
                base.Visit(step.Key == T.Id
                    ? new PropertyStep(typeof(string), step.Key, step.Value.ToString())
                    : step);
            }
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IGremlinQuery query)
        {
            var groovyBuilder = new IntIdToStringIdGroovyGremlinQueryElementVisitor();

            groovyBuilder
                .Visit(query);

            return (groovyBuilder.Builder.ToString(), groovyBuilder.GetVariables());
        }
    }
}
