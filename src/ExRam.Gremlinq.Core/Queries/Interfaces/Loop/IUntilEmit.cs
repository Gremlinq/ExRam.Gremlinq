#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IUntilEmit<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IUntilEmitRepeat<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
