using System;

namespace ExRam.Gremlinq.Core
{
    public interface IStage2ChooseBuilder<out TSourceQuery, in TElement>
        where TSourceQuery : IGremlinQuery
    {
        IStage3ChooseBuilder<TSourceQuery, TElement, TTargetQuery> Case<TTargetQuery>(TElement element, Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        ITerminalChooseBuilder<TTargetQuery> Default<TTargetQuery>(Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
    }
}
