using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Core.Tests
{
    internal static class StringExtensions
    {
        private static readonly Regex GuidRegex = new("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase);

        public static string ScrubGuids(this string str)
        {
            return GuidRegex.Replace(str, "Scrubbed GUID");
        }
    }
}
