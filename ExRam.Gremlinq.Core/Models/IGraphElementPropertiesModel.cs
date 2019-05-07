using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyIdentifierMapping IdentifierMapping { get; }
        IImmutableDictionary<MemberInfo, MemberMetadata> MetaData { get; }
    }
}
