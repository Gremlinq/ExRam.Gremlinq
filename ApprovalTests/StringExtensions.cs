using Verify;
using VerifyXunit;

namespace ExRam.Gremlinq.ApprovalTests
{
    public static class StringExtensions
    {
        public static void VerifyCSharp(this string s, VerifyBase verifyBase)
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            verifyBase.Verify(s, verifySettings);
        }
    }
}
