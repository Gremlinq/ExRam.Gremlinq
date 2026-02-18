using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TNode1, TNode2, TNode3, TNode4, TNode5, TNode6, TNode7, TNode8, TNode9, TNode10, TNode11, TNode12, TNode13, TNode14, TNode15, TNode16, TNode17, TNode18, TNode19, TNode20, TNode21, TNode22, TNode23, TNode24, TNode25, TNode26, TNode27>
        {
            private readonly ImmutableStack<TreeStep.ByStep> _bySteps;
            private readonly GremlinQuery<T1, T2, T3, T4> _sourceQuery;

            public TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery) : this(sourceQuery, ImmutableStack<TreeStep.ByStep>.Empty)
            {

            }

            private TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery, ImmutableStack<TreeStep.ByStep> bySteps)
            {
                _bySteps = bySteps;
                _sourceQuery = sourceQuery;
            }

            private IGremlinQuery<TTree> Build<TTree>() => _sourceQuery
                .Continue()
                .Build(
                    static (builder, @this) =>
                    {
                        var bySteps = @this._bySteps;

                        builder = builder
                            .AddStep(TreeStep.Instance);

                        static FinalContinuationBuilder Recurse(FinalContinuationBuilder builder, ImmutableStack<TreeStep.ByStep> bySteps)
                        {
                            if (!bySteps.IsEmpty)
                            {
                                var popped = bySteps.Pop(out var byStep);

                                builder = Recurse(builder, popped).AddStep(byStep);
                            }

                            return builder;
                        }

                        if (bySteps.Any(byStep => byStep is not TreeStep.ByIdentityStep))
                        {
                            builder = Recurse(builder, bySteps);

                            if (bySteps.Peek() is not TreeStep.ByIdentityStep)
                            {
                                builder = builder
                                    .AddStep(TreeStep.ByIdentityStep.Instance);
                            }
                        }

                        return builder
                            .WithNewProjection(Projection.Value)
                            .BuildAs<IGremlinQuery<TTree>>();
                    },
                    this);
        }
    }
}
