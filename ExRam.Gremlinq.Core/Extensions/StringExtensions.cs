namespace ExRam.Gremlinq.Core
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string source)
        {
            if (source == null || source.Length < 2)
                return source;

            return source.Substring(0, 1).ToLower() + source.Substring(1);
        }
    }
}
