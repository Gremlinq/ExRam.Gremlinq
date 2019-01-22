namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        internal static IGremlinQuery AddStep(this IGremlinQuery query, Step step)
        {
            return query.AsAdmin().InsertStep(query.AsAdmin().Steps.Count, step);
        }
     }
}
