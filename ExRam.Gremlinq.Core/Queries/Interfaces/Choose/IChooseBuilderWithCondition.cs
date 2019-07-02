using System;

namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCondition<out TSourceQuery, in TElement>
        where TSourceQuery : IGremlinQuery
    {
        IChooseBuilderWithConditionAndCase<TSourceQuery, TElement, TTargetQuery> Case<TTargetQuery>(TElement element, Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        IChooseBuilderWithConditionOrCase<TTargetQuery> Default<TTargetQuery>(Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
    }
}
