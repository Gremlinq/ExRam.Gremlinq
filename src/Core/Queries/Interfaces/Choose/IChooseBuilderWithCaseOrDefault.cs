namespace ExRam.Gremlinq.Core
{
    /// <summary>A terminal choose builder that can be built into a final query.</summary>
    /// <typeparam name="TTargetQuery">The result query type.</typeparam>
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where TTargetQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the final choose query.
        /// </summary>
        TTargetQuery Build();
    }
}
