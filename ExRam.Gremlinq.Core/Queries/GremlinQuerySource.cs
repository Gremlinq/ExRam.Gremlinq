namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IConfigurableGremlinQuerySource g = GremlinQuery.Create<object>(GremlinQueryEnvironment.Default);
        #pragma warning restore IDE1006 // Naming Styles
    }
}
