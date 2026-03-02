using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Describes a set of graph element types (vertices or edges) along with their metadata.
    /// </summary>
    public interface IGraphElementModel
    {
        /// <summary>
        /// Scans the specified assemblies for element types that extend the base type and adds them to the model.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan.</param>
        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Configures element metadata by applying a transformation to all element types.
        /// </summary>
        /// <param name="metaDataTransformation">A function that receives each element type and its current metadata.</param>
        IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation);

        /// <summary>
        /// Configures element metadata for a specific element type.
        /// </summary>
        /// <param name="elementType">The element type to configure.</param>
        /// <param name="metaDataTransformation">A function that transforms the element's metadata.</param>
        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        /// <summary>
        /// Configures member metadata by applying a transformation to all members.
        /// </summary>
        /// <param name="transformation">A function that receives each member and its current metadata.</param>
        IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        /// <summary>
        /// Configures member metadata for a specific member.
        /// </summary>
        /// <param name="member">The member to configure.</param>
        /// <param name="transformation">A function that transforms the member's metadata.</param>
        IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        /// <summary>
        /// Attempts to get the metadata for the specified member.
        /// </summary>
        /// <param name="memberInfo">The member to look up.</param>
        /// <returns>The member's metadata, or <c>null</c> if the member is not known to the model.</returns>
        MemberMetadata? TryGetMetadata(MemberInfo memberInfo);

        /// <summary>
        /// Attempts to get the metadata for the specified element type.
        /// </summary>
        /// <param name="elementType">The element type to look up.</param>
        /// <returns>The element's metadata, or <c>null</c> if the type is not known to the model.</returns>
        ElementMetadata? TryGetMetadata(Type elementType);

        /// <summary>
        /// Gets the set of element types known to this model.
        /// </summary>
        IImmutableSet<Type> ElementTypes { get; }

        /// <summary>
        /// Gets the set of members known to this model.
        /// </summary>
        IImmutableSet<MemberInfo> Members { get; }
    }
}
