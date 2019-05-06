using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertiesModel
    {
        object GetIdentifier(Expression expression);
        IImmutableDictionary<MemberInfo, MemberMetadata> MetaData { get; }
    }
}