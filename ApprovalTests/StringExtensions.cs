using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.ApprovalTests
{
    public static class StringExtensions
    {
        public static Task VerifyCSharp(this string s, XunitContextBase contextBase)
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            return Verifier.Verify(s, verifySettings, contextBase.SourceFile);
        }
    }
}
