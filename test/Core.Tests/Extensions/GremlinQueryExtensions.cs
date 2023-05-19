namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public static async Task Verify<TElement>(this IGremlinQueryBase<TElement> query)
        {
            try
            {
                await GremlinqTestBase.Current.Fixture.Verify(query);
            }
            catch (Exception ex)
            {
                await GremlinqTestBase.Current.Verify(new
                {
                    Exception = new
                    {
                        Type = ex.GetType().Name,
                        ex.Message
                    }
                });
            }
        }
    }
}
