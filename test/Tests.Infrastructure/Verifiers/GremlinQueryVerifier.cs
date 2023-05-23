using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinQueryVerifier
    {
        private readonly string _sourceFile;

        public static readonly VerifySettings DefaultSettings = new();

        static GremlinQueryVerifier()
        {
            DefaultSettings.UniqueForTargetFrameworkAndVersion();

#if DEBUG
            DefaultSettings.AutoVerify();
#endif
        }

        protected GremlinQueryVerifier([CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;
        }

        public abstract Task Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected SettingsTask InnerVerify<T>(T value)
        {
            return Verifier.Verify(value, DefaultSettings, _sourceFile);
        }

        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;
    }
}
