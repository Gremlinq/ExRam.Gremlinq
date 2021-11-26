using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Core.Tests
{
    internal static class ScrubbersExtensions
    {
        private static readonly Regex GuidRegex = new("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase);

        public static IImmutableList<Func<string, string>> ScrubGuids(this IImmutableList<Func<string, string>> list)
        {
            return list
                .Add(x => x.ScrubGuids());
        }
    }
}
