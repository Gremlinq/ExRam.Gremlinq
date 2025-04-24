namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where TTargetQuery : IGremlinQueryBase
    {
        [Obsolete("Use Build() instead.", true)]
        TTargetQuery TargetQuery { get; }

        TTargetQuery Build();
    }
}
