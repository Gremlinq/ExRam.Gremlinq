namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal readonly struct VertexPropertyPropertiesWrapper<T>
    {
        public static readonly VertexPropertyPropertiesWrapper<T> None = new();

        private VertexPropertyPropertiesWrapper(T value)
        {
            Value = value;
            HasValue = true;
        }

        public T Value { get; }
        public bool HasValue { get; }

        public static VertexPropertyPropertiesWrapper<T> From(T value) => new(value);
    }
}
