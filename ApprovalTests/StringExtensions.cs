using System.Threading.Tasks;
using Verify;
using VerifyXunit;

namespace ExRam.Gremlinq.ApprovalTests
{
    public static class StringExtensions
    {
        public static Task VerifyCSharp(this string s, VerifyBase verifyBase)
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            return verifyBase.Verify(s, verifySettings);
        }
    }
}
