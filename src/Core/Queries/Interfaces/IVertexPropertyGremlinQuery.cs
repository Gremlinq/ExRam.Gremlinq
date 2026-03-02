using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVertexPropertyGremlinQueryBase : IElementGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IElementGremlinQuery<object> Lower();

        /// <summary>
        /// Map vertex properties to their meta-properties by key.
        /// Corresponds to the Gremlin <c>properties()</c> step.
        /// </summary>
        /// <param name="keys">The meta-property keys to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#properties-step">Reference Documentation - Properties Step</seealso>
        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);
        /// <inheritdoc cref="Properties(string[])" />
        IPropertyGremlinQuery<Property<object>> Properties(params ReadOnlySpan<string> keys);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>();

        /// <summary>
        /// Map vertex properties to their meta-property values by key.
        /// Corresponds to the Gremlin <c>values()</c> step.
        /// </summary>
        /// <typeparam name="TValue">The expected type of the meta-property values.</typeparam>
        /// <param name="keys">The meta-property keys to retrieve values for.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#values-step">Reference Documentation - Values Step</seealso>
        IGremlinQuery<TValue> Values<TValue>(params string[] keys);
        /// <inheritdoc cref="Values{TValue}(string[])" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<string> keys);

        /// <inheritdoc cref="Values{TValue}(string[])" />
        IGremlinQuery<object> Values(params string[] keys);
        /// <inheritdoc cref="Values{TValue}(string[])" />
        IGremlinQuery<object> Values(params ReadOnlySpan<string> keys);

        /// <inheritdoc cref="IElementGremlinQueryBase.ValueMap()" />
        new IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>();

        /// <summary>
        /// Map vertex properties to a dictionary of their meta-property keys and values.
        /// Corresponds to the Gremlin <c>valueMap()</c> step.
        /// </summary>
        /// <typeparam name="TValue">The expected type of the meta-property values.</typeparam>
        /// <param name="keys">The meta-property keys to include.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params string[] keys);
        /// <inheritdoc cref="ValueMap{TValue}(string[])" />
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params ReadOnlySpan<string> keys);

        /// <inheritdoc cref="ValueMap{TValue}(string[])" />
        IMapGremlinQuery<IDictionary<string, object>> ValueMap(params string[] keys);
        /// <inheritdoc cref="ValueMap{TValue}(string[])" />
        IMapGremlinQuery<IDictionary<string, object>> ValueMap(params ReadOnlySpan<string> keys);
    }

    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IElementGremlinQuery<TProperty> Lower();

        /// <summary>
        /// Access the meta-properties of the vertex property, typed as <typeparamref name="TMeta"/>.
        /// </summary>
        /// <typeparam name="TMeta">The type representing the meta-properties.</typeparam>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();

        /// <inheritdoc cref="IVertexPropertyGremlinQueryBase.Properties(string[])" />
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        /// <inheritdoc cref="IVertexPropertyGremlinQueryBase.Properties(string[])" />
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Map the vertex property to its value.
        /// Corresponds to the Gremlin <c>value()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#value-step">Reference Documentation - Value Step</seealso>
        IGremlinQuery<TValue> Value();
    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue>>;

    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IElementGremlinQuery<TProperty> Lower();

        /// <summary>
        /// Map the vertex property to its meta-properties by strongly-typed projection.
        /// Corresponds to the Gremlin <c>properties()</c> step.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property value.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#properties-step">Reference Documentation - Properties Step</seealso>
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        /// <inheritdoc cref="Properties{TMetaValue}(Expression{Func{TMeta, TMetaValue}}[])" />
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params ReadOnlySpan<Expression<Func<TMeta, TMetaValue>>> projections);

        /// <summary>
        /// Set a meta-property on the vertex property by strongly-typed projection and value.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property value.</typeparam>
        /// <param name="projection">An expression selecting the meta-property.</param>
        /// <param name="value">The meta-property value to set.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        /// <summary>
        /// Map the vertex property to its value.
        /// Corresponds to the Gremlin <c>value()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#value-step">Reference Documentation - Value Step</seealso>
        IGremlinQuery<TValue> Value();

        /// <summary>
        /// Map the vertex property to its meta-property values by strongly-typed projection.
        /// Corresponds to the Gremlin <c>values()</c> step.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties whose values to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#values-step">Reference Documentation - Values Step</seealso>
        IGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        /// <inheritdoc cref="Values{TMetaValue}(Expression{Func{TMeta, TMetaValue}}[])" />
        IGremlinQuery<TMetaValue> Values<TMetaValue>(params ReadOnlySpan<Expression<Func<TMeta, TMetaValue>>> projections);

        /// <summary>
        /// Map the vertex property to a dictionary of its meta-property keys and values.
        /// Corresponds to the Gremlin <c>valueMap()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
        new IGremlinQuery<TMeta> ValueMap();

        /// <summary>
        /// Filter the vertex property by a predicate on its meta-properties.
        /// Corresponds to the Gremlin <c>where()</c> step.
        /// </summary>
        /// <param name="predicate">A predicate expression on the vertex property's meta-properties.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>;
}
