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

        protected GremlinqTestBase(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(Settings, sourceFile)
        {
            CurrentTestBase.Value = this;
            XunitContext.Register(testOutputHelper, sourceFile);
        }

        public virtual async Task Verify<TElement>(IGremlinQueryBase<TElement> query) => await Verify(query.Debug());

        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
