namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Represents a property of a graph element (vertex or edge).
    /// </summary>
    public abstract class Property
    {
        /// <summary>
        /// Returns a string representation of the property.
        /// </summary>
        /// <returns>A string in the format "p[key->value]".</returns>
        public override string ToString()
        {
            return $"p[{Key}->{GetValue()}]";
        }

        /// <summary>
        /// Gets the value of the property as an object.
        /// </summary>
        /// <returns>The property value.</returns>
        protected internal abstract object? GetValue();

        /// <summary>
        /// Gets the key (name) of the property.
        /// </summary>
        public string? Key { get; private set; }
    }

    /// <summary>
    /// Represents a strongly-typed property of a graph element.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class Property<TValue> : Property
    {
        private TValue _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Property{TValue}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The property value.</param>
        public Property(TValue value)
        {
            _value = value;
        }

        /// <summary>
        /// Implicitly converts a value to a property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Property<TValue>(TValue value) => new(value);
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The value array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> as this conversion is only valid in expression trees.
        /// </summary>
        /// <param name="value">The property array.</param>
        /// <exception cref="NotSupportedException">Always thrown when executed.</exception>
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <inheritdoc/>
        protected internal override object? GetValue() => Value;

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when attempting to set a null value.</exception>
        public TValue Value
        {
            get => _value;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                _value = value;
            }
        }
    }
}
