using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinQueryVerifier
    {
        private static readonly VerifySettings Settings = new();

        private readonly string _sourceFile;

        static GremlinQueryVerifier()
        {
            Settings.UniqueForTargetFrameworkAndVersion();

#if DEBUG
            Settings.AutoVerify();
#endif
        }

        protected GremlinQueryVerifier([CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;
        }

        public abstract Task Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected SettingsTask InnerVerify<T>(T value)
        {
            return Verifier.Verify(value, Settings, _sourceFile);
        }

        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;
    }
}
