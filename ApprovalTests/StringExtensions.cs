using ApprovalTests;

namespace ExRam.Gremlinq.ApprovalTests
{
    public static class StringExtensions
    {
        public static void VerifyCSharp(this string s)
        {
            Approvals.Verify(new ApprovalTextWriter(s, "cs"));
        }
    }
}