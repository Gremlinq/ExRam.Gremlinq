namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where TTargetQuery : IGremlinQuery
    {
        TTargetQuery TargetQuery { get; }
    }
}
