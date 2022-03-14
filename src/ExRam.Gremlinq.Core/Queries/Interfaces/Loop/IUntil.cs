#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IUntil<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IUntilRepeat<TQuery> Repeat(Func<TQuery, TQuery> loop);
        IUntilEmit<TQuery> Emit();
    }
}
