#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IRepeatEmitLoopBuilder<TQuery> Emit();
        IRepeatUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
        IRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
