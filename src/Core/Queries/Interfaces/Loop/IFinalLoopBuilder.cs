#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IFinalLoopBuilder<out TQuery>
        where TQuery : IGremlinQueryBase
    {
        TQuery Build();
    }
}
