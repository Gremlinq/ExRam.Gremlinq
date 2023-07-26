namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class Integration<TFixture> : GremlinqFixture, IDisposable
        where TFixture : GremlinqFixture, new()
    {
        public Integration() : this(new TFixture())
        {

        }

        private Integration(TFixture inner) : base(inner.GremlinQuerySource)
        {
            Inner = inner;
        }

        public TFixture Inner { get; }

        public void Dispose()
        {
            using (Inner as IDisposable)
            {
                try
                {
                    GremlinQuerySource
                        .V()
                        .Drop()
                        .ToAsyncEnumerable()
                        .LastOrDefaultAsync(default)
                        .AsTask()
                        .Wait();
                }
                catch
                {

                }
            }
        }
    }
}
