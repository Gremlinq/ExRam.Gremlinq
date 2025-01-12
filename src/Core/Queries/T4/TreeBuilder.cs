using ExRam.Gremlinq.Core.Steps;
using System.Linq.Expressions;


namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewNode> Of<TNewNode>() where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1>
        : ITreeBuilderResult<Tree<TNode1>>
            where TNode1 : notnull
    {
        ITreeBuilder<TNode1, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNewNode> By<TNewNode>(Expression<Func<TNode1, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2>>>
            where TNode1 : notnull
            where TNode2 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNewNode> By<TNewNode>(Expression<Func<TNode2, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNewNode> By<TNewNode>(Expression<Func<TNode3, TNewNode>> expression) where TNewNode : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> Of<TNewNode>() where TNewNode : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> By<TNewNode>(Expression<Func<TNode4, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> By<TNewNode>(Expression<Func<TNode5, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> By<TNewNode>(Expression<Func<TNode6, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> By<TNewNode>(Expression<Func<TNode7, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> By<TNewNode>(Expression<Func<TNode8, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> By<TNewNode>(Expression<Func<TNode9, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> By<TNewNode>(Expression<Func<TNode10, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> By<TNewNode>(Expression<Func<TNode11, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> By<TNewNode>(Expression<Func<TNode12, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> By<TNewNode>(Expression<Func<TNode13, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> By<TNewNode>(Expression<Func<TNode14, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> By<TNewNode>(Expression<Func<TNode15, TNewNode>> expression) where TNewNode : notnull;
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

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> By<TNewNode>(Expression<Func<TNode16, TNewNode>> expression) where TNewNode : notnull;
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

            ITreeBuilder<TNode1, TNewNode> ITreeBuilder<TNode1>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNewNode> ITreeBuilder<TNode1, TNode2>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode>(_sourceQuery, _bySteps.Add(TreeStep.ByIdentityStep.Instance));
            }


            ITreeBuilder< TNewNode> ITreeBuilder<TNode1>.By<TNewNode>(Expression<Func<TNode1, TNewNode>> expression)
            {
                return new TreeBuilder<TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1,  TNewNode> ITreeBuilder<TNode1, TNode2>.By<TNewNode>(Expression<Func<TNode2, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3>.By<TNewNode>(Expression<Func<TNode3, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.By<TNewNode>(Expression<Func<TNode4, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.By<TNewNode>(Expression<Func<TNode5, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.By<TNewNode>(Expression<Func<TNode6, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode, object, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.By<TNewNode>(Expression<Func<TNode7, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode, object, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.By<TNewNode>(Expression<Func<TNode8, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode, object, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.By<TNewNode>(Expression<Func<TNode9, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode, object, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.By<TNewNode>(Expression<Func<TNode10, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode, object, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.By<TNewNode>(Expression<Func<TNode11, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode, object, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.By<TNewNode>(Expression<Func<TNode12, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode, object, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.By<TNewNode>(Expression<Func<TNode13, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode, object, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.By<TNewNode>(Expression<Func<TNode14, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode, object, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.By<TNewNode>(Expression<Func<TNode15, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode, object>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15,  TNewNode> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>.By<TNewNode>(Expression<Func<TNode16, TNewNode>> expression)
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode>(_sourceQuery, _bySteps.RemoveAt(_bySteps.Count - 1).Add(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));
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

