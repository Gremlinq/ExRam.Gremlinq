using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface ITreeNodeBuilder<TNode>
    {
        ITreeNodeBuilder<TNode> By<TKey>(Expression<Func<TNode, TKey>> expression);
    }
}
