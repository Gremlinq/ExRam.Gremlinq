#pragma warning disable 660,661
#pragma warning disable IDE0060
using System;

namespace ExRam.Gremlinq.Core
{
    public abstract class StepLabel : IEquatable<StepLabel>
    {
        protected StepLabel() : this(new object())
        { 
        }

        internal StepLabel(object identity)
        {
            Identity = identity;
        }

        public virtual StepLabel<TNewValue> Cast<TNewValue>() => new(Identity);

        public bool Equals(StepLabel? other) => Identity.Equals(other?.Identity);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is StepLabel other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode() => Identity.GetHashCode();

        public static bool operator ==(StepLabel? left, StepLabel? right) => Equals(left, right);

        public static bool operator !=(StepLabel? left, StepLabel? right) => !Equals(left, right);

        internal object Identity { get; }
    }

    public class StepLabel<TElement> : StepLabel
    {
        public StepLabel() : this(new object())
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        public static implicit operator TElement(StepLabel<TElement>? stepLabel) => ThrowConversion();

        public static bool operator ==(TElement? a, StepLabel<TElement>? b) => ThrowEquality();

        public static bool operator !=(TElement? a, StepLabel<TElement>? b) => ThrowEquality();

        public static bool operator ==(StepLabel<TElement>? b, TElement? a) => ThrowEquality();

        public static bool operator !=(StepLabel<TElement>? b, TElement? a) => ThrowEquality();

        public TElement Value
        {
            get => ThrowConversion();
        }

        private static bool ThrowEquality() => throw new NotImplementedException($"The equality/inequality operators on {nameof(StepLabel)} are not intended to be called. Their use is to appear in expressions only.");

        private static TElement ThrowConversion() => throw new NotImplementedException($"The conversion operator on {nameof(StepLabel)} is not intended to be called. It's use is to appear in expressions only.");
    }

    // ReSharper disable once UnusedTypeParameter
    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQueryBase
    {
        public StepLabel() : this(new object())
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

#if NET5_0 //TODO: Use OR_LATER in the future.
        public override StepLabel<IValueGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>()
        {
            return new(Identity);
        }
#else
        public new StepLabel<IValueGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>()
        {
            return new(Identity);
        }
#endif
    }
}
#pragma warning restore 660, 661
