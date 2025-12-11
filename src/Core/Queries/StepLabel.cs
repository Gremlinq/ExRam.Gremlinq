namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a label that can be used to reference a step in a Gremlin traversal.
    /// Step labels allow storing and later referencing intermediate results during query execution.
    /// </summary>
    public abstract class StepLabel : IEquatable<StepLabel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepLabel"/> class with a unique identity.
        /// </summary>
        protected StepLabel() : this(new object())
        { 
        }

        internal StepLabel(object identity)
        {
            Identity = identity;
        }

        /// <summary>
        /// Casts this step label to a label with a different value type.
        /// </summary>
        /// <typeparam name="TNewValue">The new value type for the label.</typeparam>
        /// <returns>A new step label with the specified value type.</returns>
        public virtual StepLabel<TNewValue> Cast<TNewValue>() => new(Identity);

        /// <summary>
        /// Determines whether this step label is equal to another step label.
        /// </summary>
        /// <param name="other">The step label to compare with.</param>
        /// <returns>true if the labels are equal; otherwise, false.</returns>
        public bool Equals(StepLabel? other) => Identity.Equals(other?.Identity);

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override int GetHashCode() => Identity.GetHashCode();

        /// <summary>
        /// Determines whether two step labels are equal.
        /// </summary>
        public static bool operator ==(StepLabel? left, StepLabel? right) => Equals(left, right);

        /// <summary>
        /// Determines whether two step labels are not equal.
        /// </summary>
        public static bool operator !=(StepLabel? left, StepLabel? right) => !Equals(left, right);

        /// <summary>
        /// Implicitly converts a string to a step label.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        public static implicit operator StepLabel(string str) => new StepLabel<object>(str);

        internal object Identity { get; }
    }

    /// <summary>
    /// Represents a step label with a strongly-typed element value.
    /// </summary>
    /// <typeparam name="TElement">The type of element referenced by this label.</typeparam>
    public class StepLabel<TElement> : StepLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepLabel{TElement}"/> class.
        /// </summary>
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        /// <summary>
        /// Gets the value associated with this step label.
        /// This property should only be used in expression trees and will throw if actually executed.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when accessed outside of an expression tree.</exception>
        public TElement Value => ThrowConversion();

        /// <summary>
        /// Implicitly converts a string to a step label.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        public static implicit operator StepLabel<TElement>(string str) => new(str);

        private static TElement ThrowConversion() => throw new NotImplementedException($"The conversion operator on {nameof(StepLabel)} is not intended to be called. It's use is to appear in expressions only.");
    }

    /// <summary>
    /// Represents a step label associated with a specific query type and element type.
    /// </summary>
    /// <typeparam name="TQuery">The type of query that produced this label.</typeparam>
    /// <typeparam name="TElement">The type of element referenced by this label.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepLabel{TQuery, TElement}"/> class.
        /// </summary>
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        /// <inheritdoc/>
        public override StepLabel<IGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>()
        {
            return new(Identity);
        }

        /// <summary>
        /// Implicitly converts a string to a step label.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        public static implicit operator StepLabel<TQuery, TElement>(string str) => new(str);
    }
}
