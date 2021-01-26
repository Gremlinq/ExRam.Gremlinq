using System;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public abstract class Property : IProperty
    {
        public override string ToString()
        {
            return $"p[{Key}->{GetValue()}]";
        }

        protected abstract object GetValue();

        public string? Key { get; set; }    //TODO: Remove setter? Remove nullability?

        object IProperty.Value { get => GetValue(); }
    }

    public class Property<TValue> : Property
    {
        private TValue _value;

        public Property(TValue value)
        {
            _value = value;
        }

        public static implicit operator Property<TValue>(TValue value) => new(value);
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        protected override object GetValue() => Value!;

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
