using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using VerifyTests;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        private static readonly AsyncLocal<GremlinqTestBase> CurrentTestBase = new();

        static GremlinqTestBase()
        {
            VerifierSettings.UseStrictJson();
        }

        protected GremlinqTestBase(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(CreateSettings(), sourceFile)
        {
            CurrentTestBase.Value = this;
            XunitContext.Register(testOutputHelper, sourceFile);
        }

        private static VerifySettings CreateSettings()
        {
            var settings = new VerifySettings();

            settings.UseExtension("json");
            settings.DisableDiff();

#if (DEBUG)
            settings.AutoVerify();
#endif

            return settings;
        }

        public virtual IImmutableList<Func<string, string>> Scrubbers()
        {
            return ImmutableList<Func<string, string>>.Empty;
        }

        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
