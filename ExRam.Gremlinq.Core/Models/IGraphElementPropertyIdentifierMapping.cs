using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyIdentifierMapping
    {
        object GetIdentifier(MemberInfo memberInfo);
    }
}
