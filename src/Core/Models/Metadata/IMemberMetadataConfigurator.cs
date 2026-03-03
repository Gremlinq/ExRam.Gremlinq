using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Provides a fluent API for configuring member metadata on a specific element type.
    /// </summary>
    /// <typeparam name="TElement">The element type whose members are being configured.</typeparam>
    public interface IMemberMetadataConfigurator<TElement>
    {
        /// <summary>
        /// Marks the specified property to be ignored when adding new elements.
        /// </summary>
        IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        /// <summary>
        /// Marks the specified property to be ignored when updating existing elements.
        /// </summary>
        IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        /// <summary>
        /// Marks the specified property to always be ignored during serialization.
        /// </summary>
        IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        /// <summary>
        /// Resets the serialization behaviour of the specified property to <see cref="SerializationBehaviour.Default"/>.
        /// </summary>
        IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        /// <summary>
        /// Configures the serialization name of the specified property.
        /// </summary>
        /// <param name="propertyExpression">An expression selecting the property.</param>
        /// <param name="name">The name to use during serialization.</param>
        IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name);

        /// <summary>
        /// Applies the configured metadata transformations to the specified model.
        /// </summary>
        /// <param name="model">The model to transform.</param>
        IGraphElementModel Transform(IGraphElementModel model);
    }
}
