using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertiesModel
    {
        object GetIdentifier(Expression expression);
        IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData { get; }
    }

    public interface IGraphModel
    {
        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
        IGraphElementPropertiesModel PropertiesModel { get; }
    }
}
