#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IStartLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
    {
        IRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
        IEmitLoopBuilder<TQuery> Emit();
        IUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }
}
