using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GremlinQueryVerifier
    {
        private readonly string _sourceFile;

        public static readonly VerifySettings DefaultSettings = new();

        static GremlinQueryVerifier()
        {
            DefaultSettings.UniqueForTargetFrameworkAndVersion();
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
