using System;

namespace ExRam.Gremlinq.Core
{
    public interface IStage3ChooseBuilder<out TSourceQuery, in TElement, TTargetQuery> : ITerminalChooseBuilder<TTargetQuery>
        where TSourceQuery : IGremlinQuery where TTargetQuery : IGremlinQuery
    {
        IStage3ChooseBuilder<TSourceQuery, TElement, TTargetQuery> Case(TElement element, Func<TSourceQuery, TTargetQuery> continuation);
        ITerminalChooseBuilder<TTargetQuery> Default(Func<TSourceQuery, TTargetQuery> continuation);
    }
}
