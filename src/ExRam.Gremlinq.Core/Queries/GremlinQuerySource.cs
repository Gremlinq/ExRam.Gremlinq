namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IConfigurableGremlinQuerySource g = GremlinQuery.Create(GremlinQueryEnvironment.Default);
    }
}
