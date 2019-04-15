using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Type[] GetTypes(string label);

        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }

        object GetIdentifier(Expression expression);

        object GetIdentifier(Type elementType, string memberName);
    }
}
