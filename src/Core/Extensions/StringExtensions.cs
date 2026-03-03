namespace ExRam.Gremlinq.Core
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string source) => source.Length < 2
            ? source
            : string
                .Create(
                    source.Length, 
                    source, 
                    static (span, source) =>
                    {
                        span[0] = char.ToLower(source[0]);

                        source[1..]
                            .CopyTo(span[1..]);
                    });
    }
}
