#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IEmitLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
    {
        IEmitRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
