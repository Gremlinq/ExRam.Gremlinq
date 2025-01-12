namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }
}
