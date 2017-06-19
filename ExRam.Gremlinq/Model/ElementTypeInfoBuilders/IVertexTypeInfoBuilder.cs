using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public interface IVertexTypeInfoBuilder<T>
    {
        VertexTypeInfo Build();
        IVertexTypeInfoBuilder<T> Label(string label);
        IVertexTypeInfoBuilder<T> SecondaryIndex(Expression<Func<T, object>> indexExpression);
    }
}