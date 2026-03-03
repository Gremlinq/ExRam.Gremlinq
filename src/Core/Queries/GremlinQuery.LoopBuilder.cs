#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
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
            IUntilRepeatEmitLoopBuilder<TQuery> where TQuery : class, IGremlinQueryBase
        {
            private readonly GremlinQuery<T1, T2, T3, T4> _outerQuery;

            public LoopBuilder(GremlinQuery<T1, T2, T3, T4> outerQuery)
            {
                _outerQuery = outerQuery;
            }

            IRepeatEmitLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Emit() => Emit();

            IUntilEmitLoopBuilder<TQuery> IUntilLoopBuilder<TQuery>.Emit() => Emit();

            IUntilRepeatEmitLoopBuilder<TQuery> IUntilRepeatLoopBuilder<TQuery>.Emit() => Emit();

            IEmitRepeatLoopBuilder<TQuery> IEmitLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop)
            {
                ArgumentNullException.ThrowIfNull(loop);

                return Repeat(loop);
            }

            IUntilRepeatLoopBuilder<TQuery> IUntilLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop)
            {
                ArgumentNullException.ThrowIfNull(loop);

                return Repeat(loop);
            }

            IUntilEmitRepeatLoopBuilder<TQuery> IUntilEmitLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop)
            {
                ArgumentNullException.ThrowIfNull(loop);

                return Repeat(loop);
            }

            IEmitRepeatUntilLoopBuilder<TQuery> IEmitRepeatLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition)
            {
                ArgumentNullException.ThrowIfNull(condition);

                return Until(condition);
            }

            IRepeatUntilLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition)
            {
                ArgumentNullException.ThrowIfNull(condition);

                return Until(condition);
            }

            IRepeatEmitUntilLoopBuilder<TQuery> IRepeatEmitLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition)
            {
                ArgumentNullException.ThrowIfNull(condition);

                return Until(condition);
            }

            IRepeatLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Repeat(Func<TQuery, TQuery> loop)
            {
                ArgumentNullException.ThrowIfNull(loop);

                return Repeat(loop);
            }

            IEmitLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Emit() => Emit();

            IUntilLoopBuilder<TQuery> IStartLoopBuilder<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition)
            {
                ArgumentNullException.ThrowIfNull(condition);

                return Until(condition);
            }

            IEmitRepeatUntilLoopBuilder<TQuery> IEmitRepeatLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            IRepeatUntilLoopBuilder<TQuery> IRepeatLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            IRepeatEmitUntilLoopBuilder<TQuery> IRepeatEmitLoopBuilder<TQuery>.Times(int loopCount) => Times(loopCount);

            private LoopBuilder<TQuery> Emit() => new(_outerQuery
                .Continue()
                .Build(static builder => builder
                    .AddStep(EmitStep.Instance)
                    .BuildAuto<T1, T2, T3, T4>()));

            private LoopBuilder<TQuery> Times(int loopCount) => new(_outerQuery
                .Continue()
                .Build(
                    static (builder, loopCount) => builder
                        .AddStep(new TimesStep(loopCount))
                        .BuildAuto<T1, T2, T3, T4>(),
                    loopCount));

            private LoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> untilCondition) => new(_outerQuery
                .Continue()
                .With(
                    static (__, untilCondition) => untilCondition(Unsafe.As<TQuery>(__)),
                    untilCondition)
                .Build(static (builder, innerTraversal) =>
                {
                    if (!innerTraversal.IsNone())
                    {
                        builder = builder
                            .AddStep(new UntilStep(innerTraversal));
                    }

                    return builder
                        .BuildAuto<T1, T2, T3, T4>();
                }));

            private LoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop) => new(_outerQuery
                .Continue()
                .With(
                    static (__, loop) => loop(Unsafe.As<TQuery>(__)),
                    loop)
                .Build(
                    static (builder, innerTraversal) => builder
                        .AddStep(new RepeatStep(innerTraversal))
                        .WithNewProjection(
                            static (projection, otherProjection) => projection.Lowest(otherProjection),
                            innerTraversal.Projection)
                        .BuildAuto<T1, T2, T3, T4>()));

            public TQuery Build() => Unsafe.As<TQuery>(_outerQuery);
        }
    }
}
