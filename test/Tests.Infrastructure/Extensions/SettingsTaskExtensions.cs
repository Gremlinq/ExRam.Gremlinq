using System.Text.RegularExpressions;
using static ExRam.Gremlinq.Tests.Infrastructure.SourceFileName;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static partial class SettingsTaskExtensions
    {
        public static SettingsTask ScrubRegex(this SettingsTask task, Regex regex, string replacement) => task.ScrubLinesWithReplace(str => regex.Replace(str, replacement));

        public static SettingsTask ScrubGuidsWithConstant(this SettingsTask task) => task.ScrubRegex(GuidRegex(), "12345678-9012-3456-7890-123456789012");

        public static SettingsTask UseSnapshotDirectoryAndNameOf<T>(this SettingsTask task) where T : ISourceFileNameProvider<T>
        {
            if (Path.GetDirectoryName(Of<T>()) is { } directory)
            {
                return task
                    .UseDirectory(directory)
                    .UseTypeName(typeof(T).Name)
                    .DisableRequireUniquePrefix();
            }

            throw new InvalidOperationException();
        }

        [GeneratedRegex("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase | RegexOptions.Compiled, "de-DE")]
        private static partial Regex GuidRegex();
    }
}
