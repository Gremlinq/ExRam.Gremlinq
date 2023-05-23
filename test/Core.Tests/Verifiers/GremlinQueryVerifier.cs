using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinQueryVerifier
    {
        public sealed class DefaultGremlinQueryVerifier : GremlinQueryVerifier
        {
            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
        }

        public static readonly GremlinQueryVerifier Default = new DefaultGremlinQueryVerifier();

        public abstract Task Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected SettingsTask InnerVerify<T>(T value) => GremlinqTestBase.Current.Verify(value);

        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;
    }
}
