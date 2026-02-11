using System.Text;

namespace ExRam.Gremlinq.Core.Generators
{
    internal static class ArgumentListExtensions
    {
        public static string GetArgumentList(string template, int argumentCount, bool hasPreceedingArguments = false, bool hasFollowingArguments = false)
        {
            return GetArgumentList(template, ", ", argumentCount, hasPreceedingArguments, hasFollowingArguments);
        }

        public static string GetArgumentList(string template, string separator, int argumentCount, bool hasPreceedingArguments = false, bool hasFollowingArguments = false)
        {
            var sb = new StringBuilder();

            if (argumentCount > 0 && hasPreceedingArguments)
                sb.Append(separator);

            for (var i = 1; i <= argumentCount; i++)
            {
                if (i > 1)
                    sb.Append(separator);

                sb.Append(string.Format(template, i));
            }

            if (argumentCount > 0 && hasFollowingArguments)
                sb.Append(separator);

            return sb.ToString();
        }

        public static string GetGenericArgumentList(string template, int argumentCount)
        {
            if (argumentCount > 0)
                return $"<{GetArgumentList(template, argumentCount)}>";

            return "";
        }
    }
}
