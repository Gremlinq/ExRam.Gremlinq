using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVPropertiesGremlinQuery<TProperty, TPropertyValue> : IGremlinQuery<TProperty>, IElementGremlinQuery
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IVPropertiesGremlinQuery<VertexProperty<TPropertyValue, TMeta>, TPropertyValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<object>> Properties(params string[] keys);
        IVPropertiesGremlinQuery<TProperty, TPropertyValue> Property(string key, object value);

        IVPropertiesGremlinQuery<TProperty, TPropertyValue> SideEffect(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue>, IGremlinQuery> sideEffectTraversal);

        IGremlinQuery<object> Values(params string[] keys);
        IGremlinQuery<IDictionary<string, object>> ValueMap();
    }

    public interface IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> : IVPropertiesGremlinQuery<TProperty, TPropertyValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value);

        IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> SideEffect(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        new IGremlinQuery<TMeta> ValueMap();
    }
}
