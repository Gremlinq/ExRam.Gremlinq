namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// A label that identifies a particular step in a Gremlin traversal, used for referencing intermediate results.
    /// </summary>
    public abstract class StepLabel : IEquatable<StepLabel>
    {
        /// <summary>Initializes a new <see cref="StepLabel"/> with a unique identity.</summary>
        protected StepLabel()
        {
            Identity = this;
        }

        internal StepLabel(object identity)
        {
            Identity = identity;
        }

        /// <summary>Casts this step label to reference a different value type.</summary>
        /// <typeparam name="TNewValue">The new value type.</typeparam>
        public virtual StepLabel<TNewValue> Cast<TNewValue>() => new(Identity);

        /// <inheritdoc />
        public bool Equals(StepLabel? other) => Identity.Equals(other?.Identity);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is StepLabel other)
                return Equals(other);

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode() => Identity.GetHashCode();

        /// <summary>Tests two step labels for equality.</summary>
        public static bool operator ==(StepLabel? left, StepLabel? right) => Equals(left, right);

        /// <summary>Tests two step labels for inequality.</summary>
        public static bool operator !=(StepLabel? left, StepLabel? right) => !Equals(left, right);

        /// <summary>Implicitly creates a step label from a string identity.</summary>
        /// <param name="str">The string identity.</param>
        public static implicit operator StepLabel(string str) => new StepLabel<object>(str);

        internal object Identity { get; }
    }

    /// <summary>
    /// A step label that carries the type of the element it references.
    /// </summary>
    /// <typeparam name="TElement">The type of the labeled element.</typeparam>
    public class StepLabel<TElement> : StepLabel
    {
        /// <summary>Initializes a new <see cref="StepLabel{TElement}"/>.</summary>
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        /// <summary>Gets the value of the labeled step. Intended for use in expressions only.</summary>
        public TElement Value => ThrowConversion();

        /// <summary>Implicitly creates a step label from a string identity.</summary>
        /// <param name="str">The string identity.</param>
        public static implicit operator StepLabel<TElement>(string str) => new(str);

        private static TElement ThrowConversion() => throw new NotImplementedException($"The conversion operator on {nameof(StepLabel)} is not intended to be called. It's use is to appear in expressions only.");
    }

    /// <summary>
    /// A step label that carries both the element type and the query type it originated from.
    /// </summary>
    /// <typeparam name="TQuery">The query type that produced the labeled step.</typeparam>
    /// <typeparam name="TElement">The type of the labeled element.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQueryBase
    {
        /// <summary>Initializes a new <see cref="StepLabel{TQuery, TElement}"/>.</summary>
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        /// <inheritdoc />
        public override StepLabel<IGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>() => new(Identity);

        /// <summary>Implicitly creates a step label from a string identity.</summary>
        /// <param name="str">The string identity.</param>
        public static implicit operator StepLabel<TQuery, TElement>(string str) => new(str);
    }
}
