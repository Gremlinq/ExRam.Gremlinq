using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public abstract class GremlinqTestBase
    {
        private static readonly AsyncLocal<GremlinqTestBase> CurrentTestBase = new();

        protected GremlinqTestBase(GremlinQueryVerifier verifier, ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "")
        {
            Verifier = verifier;
            CurrentTestBase.Value = this;
            XunitContext.Register(testOutputHelper, sourceFile);
        }

        public GremlinQueryVerifier Verifier { get; }
        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
