using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class ScrubbersExtensions
    {
        public static IImmutableList<Func<string, string>> ScrubGuids(this IImmutableList<Func<string, string>> list)
        {
            return list
                .Add(x => x.ScrubGuids());
        }
    }
}
