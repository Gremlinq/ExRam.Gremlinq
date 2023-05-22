using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        private static readonly VerifySettings Settings = new ();
        private static readonly AsyncLocal<GremlinqTestBase> CurrentTestBase = new();

        static GremlinqTestBase()
        {
            Settings.UniqueForTargetFrameworkAndVersion();

#if DEBUG
            Settings.AutoVerify();
#endif
        }

        protected GremlinqTestBase(GremlinqTestFixture fixture, GremlinQueryVerifier verifier, ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(Settings, sourceFile)
        {
            Fixture = fixture;
            Verifier = verifier;
            CurrentTestBase.Value = this;
            XunitContext.Register(testOutputHelper, sourceFile);
        }

        public GremlinqTestFixture Fixture { get; }
        public GremlinQueryVerifier Verifier { get; }
        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
