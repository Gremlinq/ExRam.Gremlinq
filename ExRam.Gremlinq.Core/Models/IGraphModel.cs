using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Type[] GetTypes(string label);

        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }

        object GetIdentifier(Expression expression);

        PropertyMetadata GetPropertyMetadata(PropertyInfo property);

        IGraphModel ConfigureElement<TElement>(Action<IElementConfigurator<TElement>> action);
    }
}
