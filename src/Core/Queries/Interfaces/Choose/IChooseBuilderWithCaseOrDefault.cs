namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where TTargetQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the final choose query.
        /// </summary>
        TTargetQuery Build();
    }
}
