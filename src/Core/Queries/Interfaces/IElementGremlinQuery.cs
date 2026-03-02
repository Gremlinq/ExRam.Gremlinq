using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IElementGremlinQueryBase :
        IGremlinQueryBase
    {
        /// <inheritdoc cref="IGremlinQueryBase.Cast{TResult}" />
        new IElementGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Map elements to their identifiers.
        /// Corresponds to the Gremlin <c>id()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#id-step">Reference Documentation - Id Step</seealso>
        IGremlinQuery<object> Id();

        /// <summary>
        /// Map elements to their labels.
        /// Corresponds to the Gremlin <c>label()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#label-step">Reference Documentation - Label Step</seealso>
        IGremlinQuery<string> Label();

        /// <summary>
        /// Map elements to their property values.
        /// Corresponds to the Gremlin <c>values()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#values-step">Reference Documentation - Values Step</seealso>
        IGremlinQuery<object> Values();

        /// <inheritdoc cref="Values()" />
        IGremlinQuery<TValue> Values<TValue>();

        /// <summary>
        /// Map elements to a dictionary of their property keys and values.
        /// Corresponds to the Gremlin <c>valueMap()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
        IMapGremlinQuery<IDictionary<string, object>> ValueMap();

        /// <inheritdoc cref="ValueMap()" />
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>();
    }

    public interface IElementGremlinQueryBaseRec<TSelf> :
        IElementGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Set a property on an element by key and value.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <param name="key">The property key.</param>
        /// <param name="value">The property value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        TSelf Property(string key, object? value);

        /// <summary>
        /// Set a property on an element by key, with its value resolved from a traversal.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <param name="key">The property key.</param>
        /// <param name="valueTransformation">A traversal that resolves the property value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        TSelf Property(string key, Func<TSelf, IGremlinQueryBase> valueTransformation);
    }

    public interface IElementGremlinQueryBase<TElement> :
        IElementGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Update the properties of elements in the traversal from the given object.
        /// </summary>
        /// <param name="element">The element whose property values will be applied.</param>
        IElementGremlinQuery<TElement> Update(TElement element);

        /// <inheritdoc cref="IElementGremlinQueryBase.ValueMap()" />
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params Expression<Func<TElement, TValue>>[] keys);

        /// <inheritdoc cref="IElementGremlinQueryBase.ValueMap()" />
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue>>> keys);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TElement, TValue>>[] projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TElement, TValue[]>>[] projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue[]>>> projections);
    }

    public interface IElementGremlinQueryBaseRec<TElement, TSelf> :
        IElementGremlinQueryBaseRec<TSelf>,
        IElementGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        /// <summary>
        /// Filter elements based on a property traversal.
        /// Corresponds to the Gremlin <c>where()</c> step applied via a property projection.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projected property.</typeparam>
        /// <param name="projection">An expression selecting the property to filter on.</param>
        /// <param name="propertyTraversal">A traversal applied to the projected property for filtering.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
        TSelf Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyTraversal);

        /// <summary>
        /// Set a property on an element by a strongly-typed expression and value.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">An expression selecting the property.</param>
        /// <param name="value">The property value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, TProjectedValue value);

        /// <summary>
        /// Set a property on an element by a strongly-typed expression and a step label reference.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">An expression selecting the property.</param>
        /// <param name="stepLabel">A step label whose value will be used as the property value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel);

        /// <summary>
        /// Set a property on an element by a strongly-typed expression, with its value resolved from a traversal.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">An expression selecting the property.</param>
        /// <param name="valueTraversal">A traversal that resolves the property value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, Func<TSelf, IGremlinQueryBase<TProjectedValue>> valueTraversal);
    }

    public interface IElementGremlinQuery<TElement> :
        IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>;
}
