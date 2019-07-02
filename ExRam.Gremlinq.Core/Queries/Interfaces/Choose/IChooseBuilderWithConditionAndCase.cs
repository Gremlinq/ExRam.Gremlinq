using System;

namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithConditionAndCase<out TSourceQuery, in TElement, TTargetQuery> : IChooseBuilderWithConditionOrCase<TTargetQuery>
        where TSourceQuery : IGremlinQuery where TTargetQuery : IGremlinQuery
    {
        IChooseBuilderWithConditionAndCase<TSourceQuery, TElement, TTargetQuery> Case(TElement element, Func<TSourceQuery, TTargetQuery> continuation);
        IChooseBuilderWithConditionOrCase<TTargetQuery> Default(Func<TSourceQuery, TTargetQuery> continuation);
    }
}
