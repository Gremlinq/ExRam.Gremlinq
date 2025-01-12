namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewNode> Of<TNewNode>() where TNewNode : notnull;
    }
}
