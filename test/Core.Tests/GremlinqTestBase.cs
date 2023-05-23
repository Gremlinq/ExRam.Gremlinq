namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public abstract class GremlinqTestBase
    {
        private static readonly AsyncLocal<GremlinqTestBase> CurrentTestBase = new();

        protected GremlinqTestBase(GremlinQueryVerifier verifier)
        {
            Verifier = verifier;
            CurrentTestBase.Value = this;
        }

        public GremlinQueryVerifier Verifier { get; }
        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
