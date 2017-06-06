using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public interface IVertexInfoBuilder<T>
    {
        VertexInfo Build();
        IVertexInfoBuilder<T> Label(string label);
        IVertexInfoBuilder<T> SecondaryIndex(Expression<Func<T, object>> indexExpression);
        IVertexInfoBuilder<T> PrimaryKey(Expression<Func<T, object>> expression);
    }
}