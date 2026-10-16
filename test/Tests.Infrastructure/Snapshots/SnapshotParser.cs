namespace ExRam.Gremlinq.Tests.Infrastructure
{
    /// <summary>
    /// Reads Verify's relaxed JSON: keys and values unquoted, one member or item per line, <c>{}</c> and <c>[]</c>
    /// for empty containers, a comma after every member or item but the last.
    /// </summary>
    public static class SnapshotParser
    {
        /// <summary>
        /// What a document recording a <see cref="Core.GremlinQueryExecutionException"/> becomes. Another implementation
        /// of the same semantics fails with an exception of its own, so only the fact that the query failed is compared.
        /// </summary>
        public const string Exception = "EXCEPTION";

        private const string ExceptionMarker = "Type: GremlinQueryExecutionException";

        public static SnapshotNode Parse(string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            if (text.Contains(ExceptionMarker, StringComparison.Ordinal))
                return new SnapshotScalar(Exception);

            var lines = text.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return new SnapshotScalar(string.Empty);

            var index = 1;
            var node = ParseValue(lines, ref index, lines[0]);

            if (index != lines.Length)
                throw new FormatException($"Line {index + 1} follows the end of the document: '{lines[index]}'.");

            return node;
        }

        private static SnapshotNode ParseValue(string[] lines, ref int index, string token) => token switch
        {
            "{" => ParseObject(lines, ref index),
            "[" => ParseArray(lines, ref index),
            "{}" or "{}," => new SnapshotObject([]),
            "[]" or "[]," => new SnapshotArray([]),
            _ => new SnapshotScalar(token.EndsWith(',') ? token[..^1] : token)
        };

        private static SnapshotObject ParseObject(string[] lines, ref int index)
        {
            var members = new List<KeyValuePair<string, SnapshotNode>>();

            while (index < lines.Length)
            {
                var line = lines[index++];

                if (line is "}" or "},")
                    return new SnapshotObject(members);

                var separator = line.IndexOf(": ", StringComparison.Ordinal);

                if (separator < 0 && line.EndsWith(':'))
                    separator = line.Length - 1;

                if (separator < 0)
                    throw new FormatException($"Line {index} is no member: '{line}'.");

                members.Add(new(line[..separator], ParseValue(lines, ref index, line[(separator + 1)..].TrimStart())));
            }

            throw new FormatException("The document ends inside an object.");
        }

        private static SnapshotArray ParseArray(string[] lines, ref int index)
        {
            var items = new List<SnapshotNode>();

            while (index < lines.Length)
            {
                var line = lines[index++];

                if (line is "]" or "],")
                    return new SnapshotArray(items);

                items.Add(ParseValue(lines, ref index, line));
            }

            throw new FormatException("The document ends inside an array.");
        }
    }
}
