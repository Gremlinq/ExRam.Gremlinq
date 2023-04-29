#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IUntilEmitLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        IUntilEmitRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
