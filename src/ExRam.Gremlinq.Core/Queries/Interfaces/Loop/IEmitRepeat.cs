#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

using System;

namespace ExRam.Gremlinq.Core
{
    public interface IEmitRepeat<TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IEmitRepeatUntil<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }
}
