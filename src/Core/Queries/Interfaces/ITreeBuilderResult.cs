namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilderResult<TTree>
        where TTree : ITree
    {
        IGremlinQuery<TTree> Build();
    }
}
