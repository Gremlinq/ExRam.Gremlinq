namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyScript
    {
        private readonly string? _value;

        private GroovyScript(string value)
        {
            _value = value;
        }

        public static GroovyScript From(string value) => new(value);

        public string Value => _value ?? throw new InvalidOperationException();
    }
}
