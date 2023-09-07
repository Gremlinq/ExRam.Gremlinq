using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        public static SettingsTask Verify<TElement>(this IGremlinQueryBase<TElement> query) => GremlinqTestBase.Current.Verifier.Verify(query);
    }
}
