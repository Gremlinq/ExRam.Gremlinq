namespace ExRam.Gremlinq.Core.Serialization
{
    internal readonly struct StepLabelName
    {
        private readonly string? _stringKey;
        private static readonly StepLabelName[]? Keys = Enumerable.Range(0, 100)
            .Select(static x => (StepLabelName)x)
            .ToArray();

        public StepLabelName(string key)
        {
            _stringKey = key;
        }

        public StepLabelName(int key)
        {
            if (key >= 0 && key < Keys?.Length)
                _stringKey = Keys[key]._stringKey;
            else
            {
                var digits = key > 0
                    ? (int)Math.Ceiling(Math.Log(key + 1, 10)) + 1
                    : 2;

                _stringKey = string.Create(
                    digits,
                    (key, digits),
                    static (span, tuple) =>
                    {
                        var (key, digits) = tuple;

                        span[0] = 'l';

                        for (var i = digits - 1; i >= 1; i--)
                        {
                            span[i] = (char)('0' + key % 10);
                            key /= 10;
                        }
                    });
            }
        }

        public static implicit operator StepLabelName(int key) => new(key);

        public static implicit operator StepLabelName(string key) => new(key);

        public static implicit operator string(StepLabelName key) => key._stringKey ?? throw new ArgumentException(null, nameof(key));

        public override string ToString() => _stringKey ?? "(invalid)";
    }

    internal readonly struct BindingKey
    {
        private readonly string? _stringKey;
        private static readonly BindingKey[]? Keys = Enumerable.Range(0, 100)
            .Select(static x => (BindingKey)x)
            .ToArray();

        public BindingKey(int key)
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

        public static implicit operator BindingKey(int key) => new(key);

        public static implicit operator string(BindingKey key) => key._stringKey ?? throw new ArgumentException(null, nameof(key));

        public override string ToString() => _stringKey ?? "(invalid)";
    }
}
