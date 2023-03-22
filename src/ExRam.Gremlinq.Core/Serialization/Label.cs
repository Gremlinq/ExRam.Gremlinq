namespace ExRam.Gremlinq.Core.Serialization
{
    internal readonly struct Label
    {
        private readonly string? _stringKey;
        private static readonly Label[]? Keys = Enumerable.Range(0, 100)
            .Select(static x => (Label)x)
            .ToArray();

        public Label(string key)
        {
            _stringKey = key;
        }

        public Label(int key)
        {
            if (key >= 0 && key < Keys?.Length)
                _stringKey = Keys[key]._stringKey;
            else
            {
                var digits = key > 0
                    ? (int)Math.Ceiling(Math.Log(key + 1, 26)) + 1
                    : 2;

                _stringKey = string.Create(
                    digits,
                    (key, digits),
                    static (span, tuple) =>
                    {
                        var (key, digits) = tuple;

                        span[0] = '_';

                        for (var i = digits - 1; i >= 1; i--)
                        {
                            span[i] = (char)('a' + key % 26);
                            key /= 26;
                        }
                    });
            }
        }

        public static implicit operator Label(int key) => new(key);

        public static implicit operator string(Label key) => key._stringKey ?? throw new ArgumentException(null, nameof(key));

        public static implicit operator Label(string key) => new(key);

        public override string ToString() => _stringKey ?? "(invalid)";
    }
}
