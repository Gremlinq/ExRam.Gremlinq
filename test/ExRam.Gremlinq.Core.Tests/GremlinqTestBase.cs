using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        protected GremlinqTestBase([CallerFilePath] string sourceFile = "") : base(CreateSettings(), sourceFile)
        {
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
