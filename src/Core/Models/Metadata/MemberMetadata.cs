using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Models
{
    public readonly struct MemberMetadata : IEquatable<MemberMetadata>
    {
        private readonly Key? _key;

        public MemberMetadata(Key key, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            _key = key;
            SerializationBehaviour = serializationBehaviour;
        }

        public Key Key => _key ?? throw new InvalidOperationException($"Cannot retrieve the {nameof(Key)} property of an uninitialized {nameof(MemberMetadata)} struct.");

        public SerializationBehaviour SerializationBehaviour { get; }

        public static MemberMetadata Default(string key) => new ("id".Equals(key, StringComparison.OrdinalIgnoreCase)
            ? T.Id
            : "label".Equals(key, StringComparison.OrdinalIgnoreCase)
                ? T.Label
                : key);

        public bool Equals(MemberMetadata other) => _key == other._key && SerializationBehaviour == other.SerializationBehaviour;

        public override bool Equals(object? obj) => obj is MemberMetadata metadata && Equals(metadata);

        public override int GetHashCode() => HashCode.Combine(_key, SerializationBehaviour);

        public static bool operator ==(MemberMetadata left, MemberMetadata right) => left.Equals(right);

        public static bool operator !=(MemberMetadata left, MemberMetadata right) => !(left == right);
    }
}
