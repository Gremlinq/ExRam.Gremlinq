using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Type[] GetTypes(string label);

        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
        IMetadataStore MetadataStore { get; }

        object GetIdentifier(Expression expression);

        IGraphModel Configure(Action<IElementBuilder> action);
    }
}
