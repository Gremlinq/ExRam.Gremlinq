using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVPropertiesGremlinQuery<TValue> : IElementGremlinQuery<VertexProperty<TValue>>
    {
        IVPropertiesGremlinQuery<TValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<object>> Properties(params string[] keys);
        IVPropertiesGremlinQuery<TValue> Property(string key, object value);

        IGremlinQuery<object> Values(params string[] keys);
        IGremlinQuery<IDictionary<string, object>> ValueMap();
    }

    public partial interface IVPropertiesGremlinQuery<TValue, TMeta> : IElementGremlinQuery<VertexProperty<TValue, TMeta>>
    {
        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVPropertiesGremlinQuery<TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IGremlinQuery<TMeta> ValueMap();
    }
}
