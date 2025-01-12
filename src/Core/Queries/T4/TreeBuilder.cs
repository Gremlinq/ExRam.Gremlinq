
namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder<TNode1>
        : ITreeBuilderResult<Tree<TNode1>>
            where TNode1 : notnull
    {
        ITreeBuilder<TNode1, TNode2> Of<TNode2>() where TNode2 : notnull;

        ITreeBuilder<TNode1, TNode2> Of<TNode2>(Func<ITreeNodeBuilder<TNode2>, ITreeNodeBuilder<TNode2>> nodeBuilderTransformation) where TNode2 : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2>>>
            where TNode1 : notnull
            where TNode2 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3> Of<TNode3>() where TNode3 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3> Of<TNode3>(Func<ITreeNodeBuilder<TNode3>, ITreeNodeBuilder<TNode3>> nodeBuilderTransformation) where TNode3 : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4> Of<TNode4>() where TNode4 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4> Of<TNode4>(Func<ITreeNodeBuilder<TNode4>, ITreeNodeBuilder<TNode4>> nodeBuilderTransformation) where TNode4 : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5> Of<TNode5>() where TNode5 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5> Of<TNode5>(Func<ITreeNodeBuilder<TNode5>, ITreeNodeBuilder<TNode5>> nodeBuilderTransformation) where TNode5 : notnull;
    }

    public interface ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>
        : ITreeBuilderResult<Tree<TNode1, Tree<TNode2, Tree<TNode3, Tree<TNode4, Tree<TNode5>>>>>>
            where TNode1 : notnull
            where TNode2 : notnull
            where TNode3 : notnull
            where TNode4 : notnull
            where TNode5 : notnull
    {
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6> Of<TNode6>() where TNode6 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6> Of<TNode6>(Func<ITreeNodeBuilder<TNode6>, ITreeNodeBuilder<TNode6>> nodeBuilderTransformation) where TNode6 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7> Of<TNode7>() where TNode7 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7> Of<TNode7>(Func<ITreeNodeBuilder<TNode7>, ITreeNodeBuilder<TNode7>> nodeBuilderTransformation) where TNode7 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8> Of<TNode8>() where TNode8 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8> Of<TNode8>(Func<ITreeNodeBuilder<TNode8>, ITreeNodeBuilder<TNode8>> nodeBuilderTransformation) where TNode8 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9> Of<TNode9>() where TNode9 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9> Of<TNode9>(Func<ITreeNodeBuilder<TNode9>, ITreeNodeBuilder<TNode9>> nodeBuilderTransformation) where TNode9 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10> Of<TNode10>() where TNode10 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10> Of<TNode10>(Func<ITreeNodeBuilder<TNode10>, ITreeNodeBuilder<TNode10>> nodeBuilderTransformation) where TNode10 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11> Of<TNode11>() where TNode11 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11> Of<TNode11>(Func<ITreeNodeBuilder<TNode11>, ITreeNodeBuilder<TNode11>> nodeBuilderTransformation) where TNode11 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12> Of<TNode12>() where TNode12 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12> Of<TNode12>(Func<ITreeNodeBuilder<TNode12>, ITreeNodeBuilder<TNode12>> nodeBuilderTransformation) where TNode12 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13> Of<TNode13>() where TNode13 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13> Of<TNode13>(Func<ITreeNodeBuilder<TNode13>, ITreeNodeBuilder<TNode13>> nodeBuilderTransformation) where TNode13 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14> Of<TNode14>() where TNode14 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14> Of<TNode14>(Func<ITreeNodeBuilder<TNode14>, ITreeNodeBuilder<TNode14>> nodeBuilderTransformation) where TNode14 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15> Of<TNode15>() where TNode15 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15> Of<TNode15>(Func<ITreeNodeBuilder<TNode15>, ITreeNodeBuilder<TNode15>> nodeBuilderTransformation) where TNode15 : notnull;
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
        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16> Of<TNode16>() where TNode16 : notnull;

        ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16> Of<TNode16>(Func<ITreeNodeBuilder<TNode16>, ITreeNodeBuilder<TNode16>> nodeBuilderTransformation) where TNode16 : notnull;
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
            ITreeBuilder<TNode1,  TNewNode2> ITreeBuilder<TNode1>.Of<TNewNode2>()
            {
                return new TreeBuilder<TNode1, TNewNode2, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1,  TNewNode2> ITreeBuilder<TNode1>.Of<TNewNode2>(Func<ITreeNodeBuilder<TNewNode2>, ITreeNodeBuilder<TNewNode2>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2,  TNewNode3> ITreeBuilder<TNode1, TNode2>.Of<TNewNode3>()
            {
                return new TreeBuilder<TNode1, TNode2, TNewNode3, object, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2,  TNewNode3> ITreeBuilder<TNode1, TNode2>.Of<TNewNode3>(Func<ITreeNodeBuilder<TNewNode3>, ITreeNodeBuilder<TNewNode3>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3,  TNewNode4> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode4>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNewNode4, object, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3,  TNewNode4> ITreeBuilder<TNode1, TNode2, TNode3>.Of<TNewNode4>(Func<ITreeNodeBuilder<TNewNode4>, ITreeNodeBuilder<TNewNode4>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4,  TNewNode5> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode5>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNewNode5, object, object, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4,  TNewNode5> ITreeBuilder<TNode1, TNode2, TNode3, TNode4>.Of<TNewNode5>(Func<ITreeNodeBuilder<TNewNode5>, ITreeNodeBuilder<TNewNode5>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5,  TNewNode6> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode6>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNewNode6, object, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5,  TNewNode6> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5>.Of<TNewNode6>(Func<ITreeNodeBuilder<TNewNode6>, ITreeNodeBuilder<TNewNode6>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6,  TNewNode7> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode7>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNewNode7, object, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6,  TNewNode7> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6>.Of<TNewNode7>(Func<ITreeNodeBuilder<TNewNode7>, ITreeNodeBuilder<TNewNode7>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7,  TNewNode8> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode8>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNewNode8, object, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7,  TNewNode8> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7>.Of<TNewNode8>(Func<ITreeNodeBuilder<TNewNode8>, ITreeNodeBuilder<TNewNode8>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8,  TNewNode9> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode9>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNewNode9, object, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8,  TNewNode9> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8>.Of<TNewNode9>(Func<ITreeNodeBuilder<TNewNode9>, ITreeNodeBuilder<TNewNode9>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9,  TNewNode10> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode10>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNewNode10, object, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9,  TNewNode10> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9>.Of<TNewNode10>(Func<ITreeNodeBuilder<TNewNode10>, ITreeNodeBuilder<TNewNode10>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10,  TNewNode11> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode11>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNewNode11, object, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10,  TNewNode11> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10>.Of<TNewNode11>(Func<ITreeNodeBuilder<TNewNode11>, ITreeNodeBuilder<TNewNode11>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11,  TNewNode12> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode12>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNewNode12, object, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11,  TNewNode12> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11>.Of<TNewNode12>(Func<ITreeNodeBuilder<TNewNode12>, ITreeNodeBuilder<TNewNode12>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12,  TNewNode13> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode13>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNewNode13, object, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12,  TNewNode13> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12>.Of<TNewNode13>(Func<ITreeNodeBuilder<TNewNode13>, ITreeNodeBuilder<TNewNode13>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13,  TNewNode14> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode14>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNewNode14, object, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13,  TNewNode14> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13>.Of<TNewNode14>(Func<ITreeNodeBuilder<TNewNode14>, ITreeNodeBuilder<TNewNode14>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14,  TNewNode15> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode15>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNewNode15, object>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14,  TNewNode15> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14>.Of<TNewNode15>(Func<ITreeNodeBuilder<TNewNode15>, ITreeNodeBuilder<TNewNode15>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15,  TNewNode16> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode16>()
            {
                return new TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNewNode16>(_sourceQuery);
            }

            ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15,  TNewNode16> ITreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15>.Of<TNewNode16>(Func<ITreeNodeBuilder<TNewNode16>, ITreeNodeBuilder<TNewNode16>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNewNode> ITreeBuilder.Of<TNewNode>()
            {
                return new TreeBuilder<TNewNode, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>(_sourceQuery);
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

