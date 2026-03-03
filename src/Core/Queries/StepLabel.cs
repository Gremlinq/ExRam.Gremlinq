namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// A label that identifies a particular step in a Gremlin traversal, used for referencing intermediate results.
    /// </summary>
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
            if (obj is null)
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

        public static implicit operator StepLabel(string str) => new StepLabel<object>(str);

        internal object Identity { get; }
    }

    /// <summary>
    /// A step label that carries the type of the element it references.
    /// </summary>
    /// <typeparam name="TElement">The type of the labeled element.</typeparam>
    public class StepLabel<TElement> : StepLabel
    {
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        public TElement Value => ThrowConversion();

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
        public StepLabel()
        {
        }

        internal StepLabel(object identity) : base(identity)
        {
        }

        public override StepLabel<IGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>() => new(Identity);

        public static implicit operator StepLabel<TQuery, TElement>(string str) => new(str);
    }
}
