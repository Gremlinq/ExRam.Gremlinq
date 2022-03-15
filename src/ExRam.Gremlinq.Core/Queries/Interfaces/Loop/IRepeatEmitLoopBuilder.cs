#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IRepeatEmitLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IRepeatEmitUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
        IRepeatEmitUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
