using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Metadata associated with a graph element member (property), including its key and serialization behavior.
    /// </summary>
    public readonly struct MemberMetadata : IEquatable<MemberMetadata>
    {
        private readonly Key? _key;

        /// <summary>
        /// Initializes a new instance of <see cref="MemberMetadata"/>.
        /// </summary>
        /// <param name="key">The key used for the member during serialization.</param>
        /// <param name="serializationBehaviour">The serialization behavior for this member.</param>
        public MemberMetadata(Key key, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            _key = key;
            SerializationBehaviour = serializationBehaviour;
        }

        /// <summary>
        /// Gets the serialization key for this member.
        /// </summary>
        public Key Key => _key ?? throw new InvalidOperationException($"Cannot retrieve the {nameof(Key)} property of an uninitialized {nameof(MemberMetadata)} struct.");

        /// <summary>
        /// Gets the serialization behavior for this member.
        /// </summary>
        public SerializationBehaviour SerializationBehaviour { get; }

        /// <summary>
        /// Creates a <see cref="MemberMetadata"/> with default settings for the given key.
        /// Keys named "id" or "label" (case-insensitive) are mapped to <see cref="T.Id"/> and <see cref="T.Label"/> respectively.
        /// </summary>
        /// <param name="key">The property key name.</param>
        public static MemberMetadata Default(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return new ("id".Equals(key, StringComparison.OrdinalIgnoreCase)
                ? T.Id
                : "label".Equals(key, StringComparison.OrdinalIgnoreCase)
                    ? T.Label
                    : key);
        }

        public bool Equals(MemberMetadata other) => _key == other._key && SerializationBehaviour == other.SerializationBehaviour;

        public override bool Equals(object? obj) => obj is MemberMetadata metadata && Equals(metadata);

        public override int GetHashCode() => HashCode.Combine(_key, SerializationBehaviour);

        public static bool operator ==(MemberMetadata left, MemberMetadata right) => left.Equals(right);

        public static bool operator !=(MemberMetadata left, MemberMetadata right) => !(left == right);
    }
}
