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
                #if NETSTANDARD2_0
                var stringKey = string.Empty;

                do
                {
                    stringKey = (char)('a' + key % 26) + stringKey;
                    key /= 26;
                } while (key > 0);

                _stringKey = "_" + stringKey;
                #else
                var digits = key > 0
                    ? (int)Math.Ceiling(Math.Log(key + 1, 26)) + 1
                    : 2;

                _stringKey = string.Create(
                    digits,
                    (key, digits),
                    (span, tuple) =>
                    {
                        var (key, digits) = tuple;

                        span[0] = '_';

                        for (var i = digits - 1; i >= 1; i--)
                        {
                            span[i] = (char)('a' + key % 26);
                            key /= 26;
                        }
                    });
                #endif
            }
        }

        public static implicit operator BindingKey(int key) => new(key);

        public static implicit operator string(BindingKey key) => key._stringKey is { } stringKey
            ? stringKey
            : throw new ArgumentException(null, nameof(key));

        public override string ToString() => _stringKey is { } stringKey
            ? stringKey
            : "(invalid)";
    }
}
