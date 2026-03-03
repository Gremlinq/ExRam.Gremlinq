using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents either a string property name or a TinkerPop <see cref="T"/> enum value used as a graph element key.
    /// </summary>
    public readonly struct Key : IComparable<Key>
    {
        private readonly object? _key;

        /// <summary>Initializes a <see cref="Key"/> from a TinkerPop <see cref="T"/> enum value.</summary>
        /// <param name="t">The TinkerPop <c>T</c> accessor.</param>
        public Key(T t)
        {
            ArgumentNullException.ThrowIfNull(t);

            _key = t;
        }

        /// <summary>Initializes a <see cref="Key"/> from a string property name.</summary>
        /// <param name="name">The property name.</param>
        public Key(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            _key = name;
        }

        /// <inheritdoc />
        public bool Equals(Key other) => Equals(_key, other._key);

        /// <inheritdoc />
        public int CompareTo(Key other) => _key switch
        {
            T t1 when other._key is T t2 => StringComparer.OrdinalIgnoreCase.Compare(t1.EnumValue, t2.EnumValue),
            T => -1,
            string str1 when other._key is string str2 => StringComparer.OrdinalIgnoreCase.Compare(str1, str2),
            string => 1,
            null => other._key is null ? 0 : -1,
            _ => throw new InvalidOperationException($"Cannot compare {nameof(Key)}.")
        };

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Key other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => _key != null ? _key.GetHashCode() : 0;

        /// <summary>Tests two <see cref="Key"/> instances for equality.</summary>
        public static bool operator ==(Key key1, Key key2) => key1.RawKey == key2.RawKey;

        /// <summary>Tests two <see cref="Key"/> instances for inequality.</summary>
        public static bool operator !=(Key key1, Key key2) => !(key1 == key2);

        /// <summary>Implicitly converts a <see cref="T"/> value to a <see cref="Key"/>.</summary>
        /// <param name="t">The TinkerPop <c>T</c> accessor.</param>
        public static implicit operator Key(T t) => new(t);

        /// <summary>Implicitly converts a string to a <see cref="Key"/>.</summary>
        /// <param name="name">The property name.</param>
        public static implicit operator Key(string name) => new(name);

        /// <summary>Gets the underlying key object (either a <see cref="string"/> or a <see cref="T"/>).</summary>
        public object RawKey
        {
            get
            {
                if (_key == null)
                    throw new InvalidOperationException($"Cannot access the {nameof(RawKey)} property on an uninitialized {nameof(Key)}.");

                return _key;
            }
        }
    }
}
