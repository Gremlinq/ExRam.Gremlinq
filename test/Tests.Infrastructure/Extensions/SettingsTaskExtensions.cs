using System.Text.RegularExpressions;

namespace VerifyTests
{
    public static class SettingsTaskExtensions
    {
        public static SettingsTask ScrubRegex(this SettingsTask task, Regex regex, string replacement)
        {
            return task
                .ScrubLinesWithReplace(str => regex.Replace(str, replacement));
        }
    }
}
