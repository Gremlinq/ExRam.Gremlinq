using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public interface IVertexInfoBuilder<T>
    {
        VertexInfo Build();
        IVertexInfoBuilder<T> Label(string label);
        IVertexInfoBuilder<T> PrimaryKey(Expression<Func<T, object>> expression);
    }
}