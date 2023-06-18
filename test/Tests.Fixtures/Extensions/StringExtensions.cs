using Newtonsoft.Json.Linq;

namespace System
{
    internal static class StringExtensions
    {
        public static string FormatJson(this string json) => JToken.Parse(json).ToString();
    }
}
