using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyIdentifierMapping
    {
        object ToIdentifier(MemberInfo memberInfo);
    }
}