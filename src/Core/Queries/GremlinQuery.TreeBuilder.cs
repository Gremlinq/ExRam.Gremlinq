using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16>
        {
            private readonly GremlinQuery<T1, T2, T3, T4> _sourceQuery;

            public TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery)
            {
                _sourceQuery = sourceQuery;
            }

            ITreeBuilder<TNewNode> ITreeBuilder.Of<TNewNode>(Func<ITreeNodeBuilder<TNewNode>, ITreeNodeBuilder<TNewNode>> nodeBuilderTransformation)
            {
                throw new NotImplementedException();
            }

            private IGremlinQuery<TTree> Build<TTree>() => _sourceQuery
                .Continue()
                .Build(
                    static (builder, state) => builder
                        .AddStep(TreeStep.Instance)
                        .As<IGremlinQuery<TTree>>()
                        .Build(),
                    0);
        }
    }
}
