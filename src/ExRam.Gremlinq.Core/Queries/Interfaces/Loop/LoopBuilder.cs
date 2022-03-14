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
            IEmit<TQuery>,
            IEmitRepeat<TQuery>,
            IEmitRepeatUntil<TQuery>,
            IFinalLoopBuilder<TQuery>,
            IRepeat<TQuery>,
            IRepeatEmit<TQuery>,
            IRepeatEmitUntil<TQuery>,
            IRepeatUntil<TQuery>,
            IUntil<TQuery>,
            IUntilEmit<TQuery>,
            IUntilEmitRepeat<TQuery>,
            IUntilRepeat<TQuery>,
            IUntilRepeatEmit<TQuery> where TQuery : IGremlinQueryBase
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _outerQuery;

            public LoopBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> outerQuery)
            {
                _outerQuery = outerQuery;
            }

            IRepeatEmit<TQuery> IRepeat<TQuery>.Emit() => Emit();

            IUntilEmit<TQuery> IUntil<TQuery>.Emit() => Emit();

            IUntilRepeatEmit<TQuery> IUntilRepeat<TQuery>.Emit() => Emit();

            IEmitRepeat<TQuery> IEmit<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilRepeat<TQuery> IUntil<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilEmitRepeat<TQuery> IUntilEmit<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IEmitRepeatUntil<TQuery> IEmitRepeat<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatUntil<TQuery> IRepeat<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatEmitUntil<TQuery> IRepeatEmit<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeat<TQuery> IStartLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IEmit<TQuery> IStartLoopBuilder<TQuery>.Emit() => Emit();

            IUntil<TQuery> IStartLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            private LoopBuilder<TQuery> Emit() => new(_outerQuery
                .Continue()
                .Build(static builder => builder
                    .AddStep(EmitStep.Instance)
                    .Build()));

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
