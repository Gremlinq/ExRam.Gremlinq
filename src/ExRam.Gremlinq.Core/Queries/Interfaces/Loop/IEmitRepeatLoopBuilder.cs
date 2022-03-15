#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IEmitRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IEmitRepeatUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
        IEmitRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
