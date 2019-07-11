using System;

namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCondition<out TSourceQuery, in TElement>
        where TSourceQuery : IGremlinQuery
    {
        IChooseBuilderWithCase<TSourceQuery, TElement, TTargetQuery> Case<TTargetQuery>(TElement element, Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        IChooseBuilderWithCaseOrDefault<TTargetQuery> Default<TTargetQuery>(Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
    }
}
