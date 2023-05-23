using System.Text.RegularExpressions;

namespace System
{
    internal static class StringExtensions
    {
        private static readonly Regex GuidRegex = new("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string ScrubGuids(this string str)
        {
            return GuidRegex.Replace(str, "00000000-0000-0000-0000-000000000000");
        }
    }
}
