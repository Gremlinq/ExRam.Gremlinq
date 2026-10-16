namespace ExRam.Gremlinq.Tests.Infrastructure
{
    /// <summary>
    /// A snapshot as Verify writes it: a scalar, an array or an object. An object keeps repeated keys,
    /// because Verify writes a list of key/value pairs as an object with one member per pair.
    /// </summary>
    public abstract record SnapshotNode;

    public sealed record SnapshotScalar(string Text) : SnapshotNode;

    public sealed record SnapshotArray(IReadOnlyList<SnapshotNode> Items) : SnapshotNode;

    public sealed record SnapshotObject(IReadOnlyList<KeyValuePair<string, SnapshotNode>> Members) : SnapshotNode;
}
