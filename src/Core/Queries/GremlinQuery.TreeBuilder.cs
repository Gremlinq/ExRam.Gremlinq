using System.Collections.Immutable;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27, TNode28, TNode29>
        {
            private readonly GremlinQuery<T1, T2, T3, T4> _sourceQuery;
            private readonly IImmutableList<TreeStep.ByStep> _bySteps;

            public TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery) : this(sourceQuery, ImmutableList<TreeStep.ByStep>.Empty)
            {

            }

            public TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery, IImmutableList<TreeStep.ByStep> bySteps)
            {
                _bySteps = bySteps;
                _sourceQuery = sourceQuery;
            }

            private IGremlinQuery<TTree> Build<TTree>() => _sourceQuery
                .Continue()
                .Build(
                    static (builder, @this) =>
                    {
                        builder = builder
                            .AddStep(TreeStep.Instance);

                        if (@this._bySteps.Any(byStep => byStep is not TreeStep.ByIdentityStep))
                        {
                            foreach (var byStep in @this._bySteps)
                            {
                                builder = builder
                                    .AddStep(byStep);
                            }
                        }

                        return builder
                            .As<IGremlinQuery<TTree>>()
                            .Build();
                    },
                    this);
        }
    }
}
