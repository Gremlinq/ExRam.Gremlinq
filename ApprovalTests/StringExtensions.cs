using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;

namespace ExRam.Gremlinq.ApprovalTests
{
    public static class StringExtensions
    {
        public static Task VerifyCSharp(this string s, [CallerFilePath] string sourceFile = "")
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            return Verifier.Verify(s, verifySettings, sourceFile);
        }
    }
}
