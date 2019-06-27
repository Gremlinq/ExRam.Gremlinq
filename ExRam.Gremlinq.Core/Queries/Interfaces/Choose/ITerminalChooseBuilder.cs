namespace ExRam.Gremlinq.Core
{
    public interface ITerminalChooseBuilder<out TTargetQuery>
        where TTargetQuery : IGremlinQuery
    {
        TTargetQuery TargetQuery { get; }
    }
}
