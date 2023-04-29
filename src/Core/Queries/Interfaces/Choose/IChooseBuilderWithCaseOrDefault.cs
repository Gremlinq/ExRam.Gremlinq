namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where TTargetQuery : IGremlinQueryBase
    {
        TTargetQuery TargetQuery { get; }
    }
}
