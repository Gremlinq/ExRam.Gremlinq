// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4, T5, T6>
    {
        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQuery<T1, T2, T3, T4, T5, T6>>,
            IChooseBuilderWithCondition<GremlinQuery<T1, T2, T3, T4, T5, T6>, TPickElement>,
            IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4, T5, T6>, TPickElement, TTargetQuery>
            where TTargetQuery : IGremlinQueryBase
        {
            private readonly ContinuationBuilder<GremlinQuery<T1, T2, T3, T4, T5, T6>, GremlinQuery<T1, T2, T3, T4, T5, T6>> _continuation;

            public ChooseBuilder(GremlinQuery<T1, T2, T3, T4, T5, T6> query) : this(query.Continue(), query)
            {

            }

            private ChooseBuilder(ContinuationBuilder<GremlinQuery<T1, T2, T3, T4, T5, T6>, GremlinQuery<T1, T2, T3, T4, T5, T6>> continuation, GremlinQuery<T1, T2, T3, T4, T5, T6> targetQuery)
            {
                _continuation = continuation
                    .WithOuter(targetQuery);
            }

            public IChooseBuilderWithCondition<GremlinQuery<T1, T2, T3, T4, T5, T6>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<T1, T2, T3, T4, T5, T6>, IGremlinQueryBase<TNewPickElement>> chooseContinuation)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _continuation,
                    _continuation
                        .With(chooseContinuation)
                        .Build(static (builder, traversal) => builder
                            .AddStep(new ChooseOptionTraversalStep(traversal))
                            .Build()));
            }

            public IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4, T5, T6>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<T1, T2, T3, T4, T5, T6>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build(
                            static (builder, traversal, element) => builder
                                .AddStep(new OptionTraversalStep(element, traversal))
                                .WithNewProjection(
                                    static (projection, otherProjection) => projection.Lowest(otherProjection),
                                    traversal.Projection)
                                .Build(),
                            element));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4, T5, T6>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build(static (builder, traversal) => builder
                            .AddStep(new OptionTraversalStep(default, traversal))
                            .WithNewProjection(
                                static (projection, otherProjection) => projection.Lowest(otherProjection),
                                traversal.Projection)
                            .Build()));
            }

            public IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4, T5, T6>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<T1, T2, T3, T4, T5, T6>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<T1, T2, T3, T4, T5, T6>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery => _continuation.Build(static builder => builder.Build<TTargetQuery>());
        }
    }
}
