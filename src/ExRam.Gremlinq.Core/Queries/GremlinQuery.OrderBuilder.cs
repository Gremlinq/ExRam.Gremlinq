// ReSharper disable ArrangeThisQualifier
using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class OrderBuilder : IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _query;

            public OrderBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> query)
            {
                _query = query;
            }

            GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.Build() => _query;

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            private OrderBuilder By(Expression<Func<TElement, object?>> projection, Order order) => new(_query
                .Continue()
                .Build(builder => builder
                    .AddStep(new OrderStep.ByMemberStep(_query.GetKey(projection), order))
                    .Build()));

            private OrderBuilder By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> continuation, Order order) => new(_query
                .Continue()
                .With(continuation)
                .Build(
                    static (builder, traversal, order) => builder.AddStep(new OrderStep.ByTraversalStep(traversal, order)),
                    order)
                .Build());

            private OrderBuilder By(ILambda lambda) => new(_query
                .Continue()
                .Build(builder => builder
                    .AddStep(new OrderStep.ByLambdaStep(lambda))
                    .Build()));
        }
    }
}
