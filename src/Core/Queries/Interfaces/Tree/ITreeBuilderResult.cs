namespace ExRam.Gremlinq.Core
{
    /// <summary>Represents the terminal state of a tree builder that can be built into a query.</summary>
    public interface ITreeBuilderResult<TTree>
        where TTree : ITree
    {
        /// <summary>
        /// Builds and returns the final tree query.
        /// </summary>
        IGremlinQuery<TTree> Build();
    }
}
