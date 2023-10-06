using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        public static SettingsTask Verify<TElement>(this IGremlinQueryBase<TElement> query)
        {
            var task = GremlinqTestBase.Current.Verifier.Verify(query);

#if AUTO_VERIFY
            task = task.AutoVerify();
#endif

            return task;
        }
    }
}
