namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithConditionOrCase<out TTargetQuery>
        where TTargetQuery : IGremlinQuery
    {
        TTargetQuery TargetQuery { get; }
    }
}
