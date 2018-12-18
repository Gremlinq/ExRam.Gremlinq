namespace ExRam.Gremlinq.Core.Serialization
{
    public class StringGremlinQuerySerializer<TVisitor> : IGremlinQuerySerializer<SerializedGremlinQuery>
        where TVisitor : IStringGremlinQueryElementVisitor, new()
    {
        public SerializedGremlinQuery Serialize(IGremlinQuery query)
        {
            var visitor = new TVisitor();

            visitor
                .Visit(query);

            return new SerializedGremlinQuery(visitor.GetString(), visitor.GetVariableBindings());
        }
    }
}
