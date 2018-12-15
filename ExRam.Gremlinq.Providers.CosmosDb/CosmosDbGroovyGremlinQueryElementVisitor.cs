using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public class CosmosDbGroovyGremlinQueryElementVisitor : GroovyGremlinQueryElementVisitor
    {
        public override void Visit(HasStep step)
        {
            if (step.Value is P.Within within && within.Arguments.Length == 0)
                base.Visit(new NotStep(GremlinQuery.Anonymous(GraphModel.Empty).Identity()));
            else
                base.Visit(step);
        }
    }
}
