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

        /// <summary>
        /// Verify compares the canonical forms of the received and the verified text: the order of array items and
        /// object members is ignored, and an exception is compared as <see cref="SnapshotParser.Exception"/>. For a test
        /// class that verifies against the snapshots of another implementation of the same semantics, which may answer
        /// in another order where Gremlin defines none and fail with an exception of its own.
        /// </summary>
        public static SettingsTask UseCanonicalComparer(this SettingsTask task) => task.UseStringComparer(SnapshotComparer.Canonical);

        /// <summary>
        /// Back to the exact comparison, e.g. for a fact whose query has an <c>order()</c> step under
        /// <see cref="UseCanonicalComparer"/>. The later call wins.
        /// </summary>
        public static SettingsTask UseDefaultComparer(this SettingsTask task) => task.UseStringComparer(SnapshotComparer.Ordinal);

        [GeneratedRegex("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase | RegexOptions.Compiled, "de-DE")]
        private static partial Regex GuidRegex();
    }
}
