using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Represents a model for graph elements (vertices or edges) with metadata configuration capabilities.
    /// </summary>
    public interface IGraphElementModel
    {
        /// <summary>
        /// Adds types from the specified assemblies to the element model.
        /// </summary>
        /// <param name="assemblies">The assemblies containing element types to add to the model.</param>
        /// <returns>A new element model with the additional types.</returns>
        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Configures metadata for all element types using a transformation function.
        /// </summary>
        /// <param name="metaDataTransformation">A function that transforms element metadata based on the element type.</param>
        /// <returns>A new element model with the transformed metadata.</returns>
        IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation);

        /// <summary>
        /// Configures metadata for a specific element type using a transformation function.
        /// </summary>
        /// <param name="elementType">The element type to configure.</param>
        /// <param name="metaDataTransformation">A function that transforms the element metadata.</param>
        /// <returns>A new element model with the transformed metadata.</returns>
        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        /// <summary>
        /// Configures metadata for all members using a transformation function.
        /// </summary>
        /// <param name="transformation">A function that transforms member metadata.</param>
        /// <returns>A new element model with the transformed member metadata.</returns>
        IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        /// <summary>
        /// Configures metadata for a specific member using a transformation function.
        /// </summary>
        /// <param name="member">The member to configure.</param>
        /// <param name="transformation">A function that transforms the member metadata.</param>
        /// <returns>A new element model with the transformed member metadata.</returns>
        IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        /// <summary>
        /// Attempts to get metadata for a specific member.
        /// </summary>
        /// <param name="memberInfo">The member to get metadata for.</param>
        /// <returns>The member metadata if found; otherwise, null.</returns>
        MemberMetadata? TryGetMetadata(MemberInfo memberInfo);

        /// <summary>
        /// Attempts to get metadata for a specific element type.
        /// </summary>
        /// <param name="elementType">The element type to get metadata for.</param>
        /// <returns>The element metadata if found; otherwise, null.</returns>
        ElementMetadata? TryGetMetadata(Type elementType);

        /// <summary>
        /// Gets the set of element types in this model.
        /// </summary>
        IImmutableSet<Type> ElementTypes { get; }

        /// <summary>
        /// Gets the set of members (properties and fields) in this model.
        /// </summary>
        IImmutableSet<MemberInfo> Members { get; }
    }
}
