#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IRepeatEmit<TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IRepeatEmitUntil<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
        IRepeatEmitUntil<TQuery> Times(int loopCount);
    }
}
