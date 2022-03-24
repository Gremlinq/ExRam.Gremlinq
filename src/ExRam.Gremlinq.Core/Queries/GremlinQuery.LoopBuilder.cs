#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class LoopBuilder<TQuery> :
            IStartLoopBuilder<TQuery>,
            IEmitLoopBuilder<TQuery>,
            IEmitRepeatLoopBuilder<TQuery>,
            IEmitRepeatUntilLoopBuilder<TQuery>,
            IRepeatLoopBuilder<TQuery>,
            IRepeatEmitLoopBuilder<TQuery>,
            IRepeatEmitUntilLoopBuilder<TQuery>,
            IRepeatUntilLoopBuilder<TQuery>,
            IUntilLoopBuilder<TQuery>,
            IUntilEmitLoopBuilder<TQuery>,
            IUntilEmitRepeatLoopBuilder<TQuery>,
            IUntilRepeatLoopBuilder<TQuery>,
            IUntilRepeatEmitLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _outerQuery;

            public LoopBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> outerQuery)
            {
                _outerQuery = outerQuery;
            }

            IRepeatEmitLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Emit() => Emit();

            IUntilEmitLoopBuilder<TQuery> IUntilLoopBuilder<TQuery>.Emit() => Emit();

            IUntilRepeatEmitLoopBuilder<TQuery> IUntilRepeatLoopBuilder<TQuery>.Emit() => Emit();

            IEmitRepeatLoopBuilder<TQuery> IEmitLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilRepeatLoopBuilder<TQuery> IUntilLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilEmitRepeatLoopBuilder<TQuery> IUntilEmitLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IEmitRepeatUntilLoopBuilder<TQuery> IEmitRepeatLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatUntilLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatEmitUntilLoopBuilder<TQuery> IRepeatEmitLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IEmitLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Emit() => Emit();

            IUntilLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IEmitRepeatUntilLoopBuilder<TQuery> IEmitRepeatLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            IRepeatUntilLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            IRepeatEmitUntilLoopBuilder<TQuery> IRepeatEmitLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            private LoopBuilder<TQuery> Emit() => new(_outerQuery
                .Continue()
                .Build(static builder => builder
                    .AddStep(EmitStep.Instance)
                    .Build()));

            private LoopBuilder<TQuery> Times(int loopCount) => new(_outerQuery
                .Continue()
                .Build(
                    static (builder, loopCount) => builder
                        .AddStep(new TimesStep(loopCount))
                        .Build(),
                    loopCount));

            private LoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> untilCondition) => new(_outerQuery
                .Continue()
                .With(_ => untilCondition((TQuery)(object)_))
                .Build(static (builder, innerTraversal) =>
                {
                    if (!innerTraversal.IsNone())
                    {
                        builder = builder
                            .AddStep(new UntilStep(innerTraversal));
                    }

                    return builder
                        .Build();
                }));

            private LoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop) => new(_outerQuery
                .Continue()
                .With(_ => loop((TQuery)(object)_))
                .Build(
                    static (builder, innerTraversal) => builder
                        .AddStep(new RepeatStep(innerTraversal))
                        .WithNewProjection(_ => _.Lowest(innerTraversal.Projection))
                        .Build()));

            public TQuery Build() => (TQuery)(object)_outerQuery;
        }
    }
}
