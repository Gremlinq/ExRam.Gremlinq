#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>A terminal loop builder with until, emit, and repeat steps set.</summary>
    public interface IUntilEmitRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase;
}
