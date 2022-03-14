#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

using System;

namespace ExRam.Gremlinq.Core
{
    public interface IRepeat<TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IRepeatEmit<TQuery> Emit();
        IRepeatUntil<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }
}
