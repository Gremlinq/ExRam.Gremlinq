using System;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct BindingKey
    {
        private readonly string? _stringKey;
        private static readonly BindingKey[]? Keys = Enumerable.Range(0, 100)
            .Select(x => (BindingKey)x)
            .ToArray();

        public BindingKey(int key)
        {
            if (key >= 0 && key < Keys?.Length)
                _stringKey = Keys[key]._stringKey;
            else
            {
                var stringKey = string.Empty;

                do
                {
                    stringKey = (char)('a' + key % 26) + stringKey;
                    key /= 26;
                } while (key > 0);

                _stringKey = "_" + stringKey;
            }
        }

        public static implicit operator BindingKey(int key) => new(key);

        public static implicit operator string(BindingKey key) => key._stringKey is { } stringKey
            ? stringKey
            : throw new ArgumentException();

        public override string ToString() => _stringKey is { } stringKey
            ? stringKey
            : "(invalid)";
    }
}
