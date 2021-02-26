using System.Runtime.CompilerServices;

using VerifyTests;

using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        protected GremlinqTestBase(VerifySettings? settings = null, [CallerFilePath] string sourceFile = "") : base(settings, sourceFile)
        {
        }
    }
}
