namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Base class for graph element properties.
    /// </summary>
    public abstract class Property
    {
        /// <inheritdoc />
        public override string ToString() => $"p[{Key}->{GetValue()}]";

        /// <summary>Gets the value of this property as an object.</summary>
        protected internal abstract object? GetValue();

        /// <summary>
        /// Gets the key (name) of the property.
        /// </summary>
        public string? Key { get; private set; }
    }

    /// <summary>
    /// A typed graph element property with a value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class Property<TValue> : Property
    {
        private TValue _value;

        /// <summary>Initializes a new property with the given value.</summary>
        /// <param name="value">The property value.</param>
        public Property(TValue value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        /// <summary>Implicitly converts a value to a <see cref="Property{TValue}"/>.</summary>
        /// <param name="value">The value.</param>
        public static implicit operator Property<TValue>(TValue value) => new(value);
        /// <summary>Implicit conversion from array for expression use only.</summary>
        /// <param name="value">The array value.</param>
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        /// <summary>Implicit conversion from property array for expression use only.</summary>
        /// <param name="value">The property array.</param>
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        /// <inheritdoc />
        protected internal override object? GetValue() => Value;

        /// <summary>Gets or sets the property value.</summary>
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
