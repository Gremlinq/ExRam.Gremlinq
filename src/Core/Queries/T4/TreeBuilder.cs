using ExRam.Gremlinq.Core.Steps;
using System.Linq.Expressions;


namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewNode> Of<TNewNode>() where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1>
        : ITreeBuilderResult<Tree<TNode1, Tree<object>>>
            where TNode1 : notnull
    {
        ITreeBuilder<TNode1, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNewNode> By<TNewNode>(Expression<Func<TNode1, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<object>>>>
            where TNode1 : notnull
            where TNode2 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNewNode> By<TNewNode>(Expression<Func<TNode2, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<object>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNewNode> By<TNewNode>(Expression<Func<TNode3, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<object>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> By<TNewNode>(Expression<Func<TNode4, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<object>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> By<TNewNode>(Expression<Func<TNode5, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<object>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> By<TNewNode>(Expression<Func<TNode6, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<object>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> By<TNewNode>(Expression<Func<TNode7, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<object>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> By<TNewNode>(Expression<Func<TNode8, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<object>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> By<TNewNode>(Expression<Func<TNode9, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<object>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> By<TNewNode>(Expression<Func<TNode10, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<object>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> By<TNewNode>(Expression<Func<TNode11, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<object>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> By<TNewNode>(Expression<Func<TNode12, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<object>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> By<TNewNode>(Expression<Func<TNode13, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<object>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> By<TNewNode>(Expression<Func<TNode14, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<object>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> By<TNewNode>(Expression<Func<TNode15, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<object>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> By<TNewNode>(Expression<Func<TNode16, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<object>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNewNode> By<TNewNode>(Expression<Func<TNode17, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<object>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNewNode> By<TNewNode>(Expression<Func<TNode18, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<object>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNewNode> By<TNewNode>(Expression<Func<TNode19, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<object>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNewNode> By<TNewNode>(Expression<Func<TNode20, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<object>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNewNode> By<TNewNode>(Expression<Func<TNode21, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNewNode> By<TNewNode>(Expression<Func<TNode22, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
            where TNode23 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNewNode> By<TNewNode>(Expression<Func<TNode23, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
            where TNode23 : notnull
            where TNode24 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNewNode> By<TNewNode>(Expression<Func<TNode24, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
            where TNode23 : notnull
            where TNode24 : notnull
            where TNode25 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNewNode> By<TNewNode>(Expression<Func<TNode25, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
            where TNode23 : notnull
            where TNode24 : notnull
            where TNode25 : notnull
            where TNode26 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNewNode> By<TNewNode>(Expression<Func<TNode26, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<TNode27, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
            where TNode8 : notnull
            where TNode9 : notnull
            where TNode10 : notnull
            where TNode11 : notnull
            where TNode12 : notnull
            where TNode13 : notnull
            where TNode14 : notnull
            where TNode15 : notnull
            where TNode16 : notnull
            where TNode17 : notnull
            where TNode18 : notnull
            where TNode19 : notnull
            where TNode20 : notnull
            where TNode21 : notnull
            where TNode22 : notnull
            where TNode23 : notnull
            where TNode24 : notnull
            where TNode25 : notnull
            where TNode26 : notnull
            where TNode27 : notnull
    {

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNewNode> By<TNewNode>(Expression<Func<TNode27, TNewNode>> expression) where TNewNode : notnull;
    }


    partial class GremlinQueryBase<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27> :
            ITreeBuilder<TNode1>,
            ITreeBuilder<TNode1, TNode2>,
            ITreeBuilder<TNode1, TNode2, TNode3>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26>,
            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27>,
            ITreeBuilder
                where TNode1 : notnull
                where TNode2 : notnull
                where TNode3 : notnull
                where TNode4 : notnull
                where TNode5 : notnull
                where TNode6 : notnull
                where TNode7 : notnull
                where TNode8 : notnull
                where TNode9 : notnull
                where TNode10 : notnull
                where TNode11 : notnull
                where TNode12 : notnull
                where TNode13 : notnull
                where TNode14 : notnull
                where TNode15 : notnull
                where TNode16 : notnull
                where TNode17 : notnull
                where TNode18 : notnull
                where TNode19 : notnull
                where TNode20 : notnull
                where TNode21 : notnull
                where TNode22 : notnull
                where TNode23 : notnull
                where TNode24 : notnull
                where TNode25 : notnull
                where TNode26 : notnull
                where TNode27 : notnull
        {

            ITreeBuilder<TNewNode> ITreeBuilder.Of<TNewNode>()
            {
                return new TreeBuilder<TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNewNode> ITreeBuilder<TNode1>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNewNode> ITreeBuilder<TNode1, TNode2>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNewNode, object, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNewNode, object, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNewNode, object>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNewNode>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));
            }


            ITreeBuilder< TNewNode> ITreeBuilder<TNode1>.By<TNewNode>(Expression<Func<TNode1, TNewNode>> expression)
            {
                return new TreeBuilder<TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1,  TNewNode> ITreeBuilder<TNode1, TNode2>.By<TNewNode>(Expression<Func<TNode2, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.By<TNewNode>(Expression<Func<TNode3, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.By<TNewNode>(Expression<Func<TNode4, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.By<TNewNode>(Expression<Func<TNode5, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.By<TNewNode>(Expression<Func<TNode6, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.By<TNewNode>(Expression<Func<TNode7, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.By<TNewNode>(Expression<Func<TNode8, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.By<TNewNode>(Expression<Func<TNode9, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.By<TNewNode>(Expression<Func<TNode10, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.By<TNewNode>(Expression<Func<TNode11, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.By<TNewNode>(Expression<Func<TNode12, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.By<TNewNode>(Expression<Func<TNode13, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.By<TNewNode>(Expression<Func<TNode14, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.By<TNewNode>(Expression<Func<TNode15, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>.By<TNewNode>(Expression<Func<TNode16, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17>.By<TNewNode>(Expression<Func<TNode17, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18>.By<TNewNode>(Expression<Func<TNode18, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19>.By<TNewNode>(Expression<Func<TNode19, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20>.By<TNewNode>(Expression<Func<TNode20, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21>.By<TNewNode>(Expression<Func<TNode21, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22>.By<TNewNode>(Expression<Func<TNode22, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23>.By<TNewNode>(Expression<Func<TNode23, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24>.By<TNewNode>(Expression<Func<TNode24, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNewNode, object, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25>.By<TNewNode>(Expression<Func<TNode25, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNewNode, object, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26>.By<TNewNode>(Expression<Func<TNode26, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNewNode, object>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27>.By<TNewNode>(Expression<Func<TNode27, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNewNode>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }


            IGremlinQuery<Tree<TNode1, Tree<object>>> ITreeBuilderResult<Tree<TNode1, Tree<object>>>.Build() => Build<Tree<TNode1, Tree<object>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<object>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<object>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<object>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<object>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<object>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<object>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<object>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<object>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<object>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<object>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<object>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<object>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<object>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<object>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<object>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<object>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<object>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<object>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<object>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<object>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<object>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<object>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<object>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<object>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<object>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<object>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<object>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<object>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<object>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<object>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<object>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<object>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<object>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<object>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<object>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<object>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<object>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<object>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<object>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<object>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<object>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<object>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<object>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<object>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<object>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<object>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<object>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<object>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<object>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<object>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<object>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<object>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<object>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<object>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<object>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<object>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<object>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<object>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<object>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<object>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<object>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<TNode27, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<TNode27, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16, Tree<TNode17, Tree<TNode18, Tree<TNode19, Tree<TNode20, Tree<TNode21, Tree<TNode22, Tree<TNode23, Tree<TNode24, Tree<TNode25, Tree<TNode26, Tree<TNode27, Tree<object>>>>>>>>>>>>>>>>>>>>>>>>>>>>>();
        }
    }
}

