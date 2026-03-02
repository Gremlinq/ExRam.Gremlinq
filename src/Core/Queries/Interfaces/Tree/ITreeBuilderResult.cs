namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilderResult<TTree>
        where TTree : ITree
    {
        /// <summary>
        /// Builds and returns the final tree query.
        /// </summary>
        IGremlinQuery<TTree> Build();
    }
}
