namespace ExRam.Gremlinq.Tests.Infrastructure
{
    /// <summary>String comparers for Verify's <c>UseStringComparer</c>.</summary>
    public static class SnapshotComparer
    {
        /// <summary>
        /// Compares the canonical forms of the received and the verified text, so that order is ignored and exceptions are
        /// compared as <see cref="SnapshotParser.Exception"/>. A difference is named by the first differing canonical line.
        /// </summary>
        public static Task<CompareResult> Canonical(string received, string verified, IReadOnlyDictionary<string, object> context)
        {
            var receivedLines = SnapshotCanonicalizer.Canonicalize(received).Split('\n');
            var verifiedLines = SnapshotCanonicalizer.Canonicalize(verified).Split('\n');

            if (receivedLines.SequenceEqual(verifiedLines, StringComparer.Ordinal))
                return Task.FromResult(CompareResult.Equal);

            var index = receivedLines
                .Zip(verifiedLines)
                .TakeWhile(static pair => string.Equals(pair.First, pair.Second, StringComparison.Ordinal))
                .Count();

            // Explicit '\n' instead of a raw string literal, whose line ends would follow those of the checkout.
            return Task.FromResult(CompareResult.NotEqual(
                $"The canonical forms differ first at line {index + 1}.\n" +
                $"Received: {receivedLines.ElementAtOrDefault(index) ?? "(end)"}\n" +
                $"Verified: {verifiedLines.ElementAtOrDefault(index) ?? "(end)"}"));
        }

        /// <summary>Compares the received and the verified text exactly, as Verify does without a string comparer.</summary>
        public static Task<CompareResult> Ordinal(string received, string verified, IReadOnlyDictionary<string, object> context) => Task.FromResult(new CompareResult(string.Equals(received, verified, StringComparison.Ordinal)));
    }
}
