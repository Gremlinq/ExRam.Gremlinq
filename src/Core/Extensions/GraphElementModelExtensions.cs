using System.Reflection;

using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core
{
    internal static class GraphElementModelExtensions
    {
        public static ElementMetadata GetMetadata(this IGraphElementModel model, Type elementType) => model.TryGetMetadata(elementType) ?? throw new ArgumentException($"{elementType.FullName} is not part of the model.");

        public static MemberMetadata GetMetadata(this IGraphElementModel model, MemberInfo memberInfo) => model.TryGetMetadata(memberInfo) ?? throw new ArgumentException($"{memberInfo.DeclaringType?.FullName}{memberInfo.Name} is not part of the model.");
    }
}
