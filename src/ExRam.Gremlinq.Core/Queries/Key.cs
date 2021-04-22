using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Key : IComparable<Key>
    {
        private readonly object? _key;

        public Key(T t)
        {
            _key = t;
        }

        public Key(string name)
        {
            _key = name;
        }

        public bool Equals(Key other)
        {
            return Equals(_key, other._key);
        }

        public int CompareTo(Key other)
        {
            return _key switch
            {
                T t1 when other._key is T t2 => StringComparer.OrdinalIgnoreCase.Compare(t1.EnumValue, t2.EnumValue),
                T => -1,
                string str1 when other._key is string str2 => StringComparer.OrdinalIgnoreCase.Compare(str1, str2),
                string => 1,
                null => other._key is null ? 0 : -1,
                _ => throw new InvalidOperationException()
            };
        }

        public override bool Equals(object? obj)
        {
            return obj is Key other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _key != null ? _key.GetHashCode() : 0;
        }

        public static bool operator == (Key key1, Key key2)
        {
            return key1.RawKey == key2.RawKey;
        }

        public static bool operator !=(Key key1, Key key2)
        {
            return !(key1 == key2);
        }

        public static implicit operator Key(T t)
        {
            return new(t);
        }

        public static implicit operator Key(string name)
        {
            return new(name);
        }

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
