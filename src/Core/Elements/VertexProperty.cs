using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// A vertex property with a value and meta-properties.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <typeparam name="TMeta">The type of the meta-properties.</typeparam>
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
    {
        /// <summary>Initializes a new vertex property with the given value.</summary>
        /// <param name="value">The property value.</param>
        public VertexProperty(TValue value) : base(value)
        {
            if (value is IVertexProperty)
                throw new InvalidOperationException($"Cannot assign a value of type {value.GetType().Name} to a property of type {nameof(VertexProperty<,>)}.");
        }

        /// <summary>Implicitly converts a value to a <see cref="VertexProperty{TValue, TMeta}"/>.</summary>
        /// <param name="value">The value.</param>
        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new(value);
        /// <summary>Implicit conversion from array for expression use only.</summary>
        /// <param name="value">The array value.</param>
        public static implicit operator VertexProperty<TValue, TMeta>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        /// <summary>Implicit conversion from vertex property array for expression use only.</summary>
        /// <param name="value">The vertex property array.</param>
        public static implicit operator VertexProperty<TValue, TMeta>(VertexProperty<TValue, TMeta>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <inheritdoc />
        public override string ToString() => $"vp[{Label}->{GetValue()}]";

        /// <summary>Gets the meta-properties of this vertex property.</summary>
        /// <param name="environment">The query environment.</param>
        protected virtual IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment)
        {
            if (Properties is { } properties)
            {
                foreach (var (key, maybeValue) in properties.Serialize(environment, SerializationBehaviour.Default))
                {
                    if (key.RawKey is string str && maybeValue is { } value)
                        yield return new KeyValuePair<string, object>(str, value);
                }
            }
        }

        IEnumerable<KeyValuePair<string, object>> IVertexProperty.GetProperties(IGremlinQueryEnvironment environment) => GetProperties(environment);

        /// <summary>Gets the identifier of this vertex property.</summary>
        public object? Id { get; private set; }
        /// <summary>Gets the label of this vertex property.</summary>
        public string? Label { get; private set; }
        /// <summary>Gets or sets the meta-properties object.</summary>
        public TMeta? Properties { get; set; }
    }

    /// <summary>
    /// A vertex property with a value and a dictionary of string-keyed meta-properties.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        /// <summary>Initializes a new vertex property with the given value and an empty dictionary of meta-properties.</summary>
        /// <param name="value">The property value.</param>
        public VertexProperty(TValue value) : base(value)
        {
            Properties = new Dictionary<string, object>();
        }

        /// <summary>Implicitly converts a value to a <see cref="VertexProperty{TValue}"/>.</summary>
        /// <param name="value">The value.</param>
        public static implicit operator VertexProperty<TValue>(TValue value) => new(value);
        /// <summary>Implicit conversion from array for expression use only.</summary>
        /// <param name="value">The array value.</param>
        public static implicit operator VertexProperty<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        /// <summary>Implicit conversion from vertex property array for expression use only.</summary>
        /// <param name="value">The vertex property array.</param>
        public static implicit operator VertexProperty<TValue>(VertexProperty<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <inheritdoc />
        protected override IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment) => Properties!;
    }
}
