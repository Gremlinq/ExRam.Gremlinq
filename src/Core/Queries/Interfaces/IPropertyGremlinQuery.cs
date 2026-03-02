namespace ExRam.Gremlinq.Core
{
    public interface IPropertyGremlinQueryBase : IGremlinQueryBase
    {
        /// <inheritdoc cref="IGremlinQueryBase.Cast{TResult}" />
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }

    public interface IPropertyGremlinQueryBase<TElement> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Map the property to its key.
        /// Corresponds to the Gremlin <c>key()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#key-step">Reference Documentation - Key Step</seealso>
        IGremlinQuery<string> Key();

        /// <summary>
        /// Map the property to its value.
        /// Corresponds to the Gremlin <c>value()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#value-step">Reference Documentation - Value Step</seealso>
        IGremlinQuery<object> Value();

        /// <summary>
        /// Map the property to its value, typed as <typeparamref name="TValue"/>.
        /// Corresponds to the Gremlin <c>value()</c> step.
        /// </summary>
        /// <typeparam name="TValue">The expected type of the property value.</typeparam>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#value-step">Reference Documentation - Value Step</seealso>
        IGremlinQuery<TValue> Value<TValue>();
    }

    public interface IPropertyGremlinQuery<TElement> :
        IPropertyGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>;
}
