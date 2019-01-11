using System;

namespace ExRam.Gremlinq.Core
{
    public interface IEPropertiesGremlinQuery<TProperty, TPropertyValue> : IGremlinQuery<TProperty>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEPropertiesGremlinQuery<TProperty, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IEPropertiesGremlinQuery<TProperty, TPropertyValue> SideEffect(Func<IEPropertiesGremlinQuery<TProperty, TPropertyValue>, IGremlinQuery> sideEffectTraversal);
    }
}