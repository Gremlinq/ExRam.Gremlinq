namespace System.Collections.Immutable
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
