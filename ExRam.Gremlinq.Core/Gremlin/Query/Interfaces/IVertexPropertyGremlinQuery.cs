using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexPropertyGremlinQuery<TValue> : IElementGremlinQuery<VertexProperty<TValue>>
    {
        IVertexPropertyGremlinQuery<TValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        IVertexPropertyGremlinQuery<TValue> Property(string key, object value);

        IGremlinQuery<IDictionary<string, object>> ValueMap();

        IVertexPropertyGremlinQuery<TValue> Where(Expression<Func<VertexProperty<TValue>, bool>> predicate);
    }

    public partial interface IVertexPropertyGremlinQuery<TValue, TMeta> : IElementGremlinQuery<VertexProperty<TValue, TMeta>>
    {
        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVertexPropertyGremlinQuery<TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        IGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }
}
