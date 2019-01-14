using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexPropertyGremlinQuery<TValue> : IElementGremlinQuery<VertexProperty<TValue>>
    {
        IVertexPropertyGremlinQuery<TValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<object>> Properties(params string[] keys);
        IVertexPropertyGremlinQuery<TValue> Property(string key, object value);

        IValueGremlinQuery<object> Values(params string[] keys);
        IGremlinQuery<IDictionary<string, object>> ValueMap();

        IVertexPropertyGremlinQuery<TValue> Where(Expression<Func<VertexProperty<TValue>, bool>> predicate);
    }

    public partial interface IVertexPropertyGremlinQuery<TValue, TMeta> : IElementGremlinQuery<VertexProperty<TValue, TMeta>>
    {
        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVertexPropertyGremlinQuery<TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }
}
