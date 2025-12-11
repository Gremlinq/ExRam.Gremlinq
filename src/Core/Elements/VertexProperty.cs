using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Represents a vertex property with strongly-typed value and metadata.
    /// Vertex properties in graph databases can have their own properties (meta-properties).
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <typeparam name="TMeta">The type of the metadata attached to this property.</typeparam>
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexProperty{TValue, TMeta}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <exception cref="InvalidOperationException">Thrown when the value is itself a vertex property.</exception>
        public VertexProperty(TValue value) : base(value)
        {
            if (value is IVertexProperty)
                throw new InvalidOperationException($"Cannot assign a value of type {value.GetType().Name} to a property of type {nameof(VertexProperty<,>)}.");
        }

        /// <summary>
        /// Implicitly converts a value to a vertex property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new(value);
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The value array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator VertexProperty<TValue, TMeta>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The vertex property array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator VertexProperty<TValue, TMeta>(VertexProperty<TValue, TMeta>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <summary>
        /// Returns a string representation of the vertex property.
        /// </summary>
        /// <returns>A string in the format "vp[label->value]".</returns>
        public override string ToString()
        {
            return $"vp[{Label}->{GetValue()}]";
        }

        /// <summary>
        /// Gets the meta-properties of this vertex property.
        /// </summary>
        /// <param name="environment">The query environment used for serialization.</param>
        /// <returns>An enumerable of key-value pairs representing the meta-properties.</returns>
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

        /// <summary>
        /// Gets the unique identifier of this vertex property.
        /// </summary>
        public object? Id { get; private set; }
        
        /// <summary>
        /// Gets the label of this vertex property.
        /// </summary>
        public string? Label { get; private set; }
        
        /// <summary>
        /// Gets or sets the metadata (meta-properties) attached to this vertex property.
        /// </summary>
        public TMeta? Properties { get; set; }
    }

    /// <summary>
    /// Represents a vertex property with a strongly-typed value and dictionary-based metadata.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexProperty{TValue}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The property value.</param>
        public VertexProperty(TValue value) : base(value)
        {
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Implicitly converts a value to a vertex property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator VertexProperty<TValue>(TValue value) => new(value);
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The value array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator VertexProperty<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The vertex property array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator VertexProperty<TValue>(VertexProperty<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <inheritdoc/>
        protected override IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment) => Properties!;
    }
}
