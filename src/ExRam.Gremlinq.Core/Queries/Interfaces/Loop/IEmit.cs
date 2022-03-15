#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    public interface IEmit<TQuery> where TQuery : IGremlinQueryBase
    {
        IEmitRepeat<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
