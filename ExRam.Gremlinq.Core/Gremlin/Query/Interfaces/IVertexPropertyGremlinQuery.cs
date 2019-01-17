using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue> : IElementGremlinQuery<TProperty>
    {
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        IVertexPropertyGremlinQuery<TProperty, TValue> Property(string key, object value);

        IGremlinQuery<IDictionary<string, object>> ValueMap();

        IVertexPropertyGremlinQuery<TProperty, TValue> Where(Expression<Func<VertexProperty<TValue>, bool>> predicate);
    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : IElementGremlinQuery<TProperty>
    {
        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        IGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }
}
