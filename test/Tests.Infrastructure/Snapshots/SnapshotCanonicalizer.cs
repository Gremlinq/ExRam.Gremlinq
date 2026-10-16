namespace ExRam.Gremlinq.Tests.Infrastructure
{
    /// <summary>
    /// Renders a snapshot so that two results that are equal as multisets render identically: array items and object
    /// members are sorted recursively by their own canonical text, ordinal. One node per line, indented, so that a
    /// difference can be named by its first differing line.
    /// </summary>
    public static class SnapshotCanonicalizer
    {
        private const string Indentation = "  ";

        public static string Canonicalize(string snapshot) => Canonicalize(SnapshotParser.Parse(snapshot));

        public static string Canonicalize(SnapshotNode node) => node switch
        {
            SnapshotScalar scalar => scalar.Text,
            SnapshotArray array => Render('[', array.Items.Select(Canonicalize), ']'),
            SnapshotObject obj => Render('{', obj.Members.Select(static member => $"{member.Key}: {Canonicalize(member.Value)}"), '}'),
            null => throw new ArgumentNullException(nameof(node)),
            _ => throw new ArgumentException($"{node.GetType()} is no known kind of {nameof(SnapshotNode)}.", nameof(node))
        };

        private static string Render(char open, IEnumerable<string> children, char close)
        {
            var lines = children
                .Order(StringComparer.Ordinal)
                .Select(static child => Indentation + child.Replace("\n", "\n" + Indentation, StringComparison.Ordinal))
                .ToArray();

            return lines.Length == 0
                ? $"{open}{close}"
                : $"{open}\n{string.Join('\n', lines)}\n{close}";
        }
    }
}
