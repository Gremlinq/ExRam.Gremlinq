namespace ExRam.Gremlinq.Tests.Infrastructure
{
    [TestCaseOrderer("ExRam.Gremlinq.Tests.Infrastructure.SideEffectTestCaseOrderer", "ExRam.Gremlinq.Tests.Infrastructure")]
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
