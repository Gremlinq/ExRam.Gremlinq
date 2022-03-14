#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IStartLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
    {
        IRepeat<TQuery> Repeat(Func<TQuery, TQuery> loop);
        IEmit<TQuery> Emit();
        IUntil<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }

    public interface IEmit<TQuery> where TQuery : IGremlinQueryBase
    {
        IEmitRepeat<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
