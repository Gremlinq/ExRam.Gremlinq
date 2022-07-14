namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public static Task Verify<TElement>(this IGremlinQueryBase<TElement> query) => GremlinqTestBase.Current.Verify(query);
    }
}
