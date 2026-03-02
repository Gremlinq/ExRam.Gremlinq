namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Base class for graph element properties.
    /// </summary>
    public abstract class Property
    {
        public override string ToString() => $"p[{Key}->{GetValue()}]";

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

        public Property(TValue value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        public static implicit operator Property<TValue>(TValue value) => new(value);
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        protected internal override object? GetValue() => Value;

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
