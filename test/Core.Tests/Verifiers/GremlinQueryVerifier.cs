namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinQueryVerifier
    {
        public sealed class DefaultGremlinQueryVerifier : GremlinQueryVerifier
        {

        }

        public static readonly GremlinQueryVerifier Default = new DefaultGremlinQueryVerifier();

        public virtual async Task Verify<TElement>(IGremlinQueryBase<TElement> query) => await GremlinqTestBase.Current.Verify(query.Debug());
    }
}
