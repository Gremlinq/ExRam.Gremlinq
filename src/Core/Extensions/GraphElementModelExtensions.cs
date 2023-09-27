using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    internal static class GraphElementModelExtensions
    {
        public static ElementMetadata GetMetadata(this IGraphElementModel model, Type elementType) => model.TryGetMetadata(elementType) ?? throw new ArgumentException();

        public static MemberMetadata GetMetadata(this IGraphElementModel model, MemberInfo memberInfo) => model.TryGetMetadata(memberInfo) ?? throw new ArgumentException();
    }
}
