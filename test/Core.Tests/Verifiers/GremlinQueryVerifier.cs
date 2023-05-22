using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinQueryVerifier
    {
        public sealed class DefaultGremlinQueryVerifier : GremlinQueryVerifier
        {
            public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query) => await GremlinqTestBase.Current.Verify(query.Debug());
        }

        public static readonly GremlinQueryVerifier Default = new DefaultGremlinQueryVerifier();

        public abstract Task Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;
    }
}
