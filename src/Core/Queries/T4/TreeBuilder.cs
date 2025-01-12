using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1>
        : ITreeBuilderResult<Tree<TNode1>>
            where TNode1 : notnull
    {
        ITreeBuilder<TNode1, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2>>>
            where TNode1 : notnull
            where TNode2 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7>>>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
            where TNode6 : notnull
            where TNode7 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11>>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12>>>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13>>>>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14>>>>>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15>>>>>>>>>>>>>>>>
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16>>>>>>>>>>>>>>>>>
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
    }


    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16> :
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
        {
            ITreeBuilder<TNewNode> ITreeBuilder.Of<TNewNode>()
            {
                return new TreeBuilder<TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder< TNewNode> ITreeBuilder.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNewNode> ITreeBuilder<TNode1>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1,  TNewNode> ITreeBuilder<TNode1>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNewNode> ITreeBuilder<TNode1, TNode2>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2,  TNewNode> ITreeBuilder<TNode1, TNode2>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                if (nodeBuilderTransformation(TreeNodeBuilder<TNewNode>.Identity) is TreeNodeBuilder<TNewNode> projectedNodeBuilder)
                    return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode>(_sourceQuery, _bySteps.Add(projectedNodeBuilder.Build()));

                throw new InvalidOperationException();
            }



            IGremlinQuery<Tree<TNode1>> ITreeBuilderResult<Tree<TNode1>>.Build() => Build<Tree<TNode1>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2>>>.Build() => Build<Tree<TNode1, Tree<TNode2>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15>>>>>>>>>>>>>>>>();

            IGremlinQuery<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16>>>>>>>>>>>>>>>>>.Build() => Build<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5, Tree<TNode6, Tree<TNode7, Tree<TNode8, Tree<TNode9, Tree<TNode10, Tree<TNode11, Tree<TNode12, Tree<TNode13, Tree<TNode14, Tree<TNode15, Tree<TNode16>>>>>>>>>>>>>>>>>();
        }
    }
}

