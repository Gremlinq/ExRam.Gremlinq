#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IUntilLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IUntilRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
        IUntilEmitLoopBuilder<TQuery> Emit();
    }
}
