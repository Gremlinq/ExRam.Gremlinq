using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class SettingsTaskExtensions
    {
        private static readonly Regex GuidRegex = new("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static SettingsTask ScrubRegex(this SettingsTask task, Regex regex, string replacement)
        {
            return task
                .ScrubLinesWithReplace(str => regex.Replace(str, replacement));
        }

        public static SettingsTask ScrubGuids(this SettingsTask task)
        {
            return task
                .ScrubRegex(GuidRegex, "12345678-9012-3456-7890-123456789012");
        }
    }
}
