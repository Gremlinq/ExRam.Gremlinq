using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        protected GremlinqTestBase(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(CreateSettings(), sourceFile)
        {
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
    }
}
