// ReSharper disable ArrangeThisQualifier
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>
    {
        private sealed class OrderBuilder : IOrderBuilderWithBy<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>
        {
            private readonly GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery> _query;

            public OrderBuilder(GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery> query)
            {
                _query = query;
            }

            GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery> IOrderBuilderWithBy<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.Build() => _query;

            IOrderBuilderWithBy<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.By(Expression<Func<T1, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.By(Func<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.By(Func<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.ByDescending(Expression<Func<T1, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<T1, GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.ByDescending(Func<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>> IOrderBuilder<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>>.ByDescending(Func<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            private OrderBuilder By(Expression<Func<T1, object?>> projection, Order order) => new(_query
                .Continue()
                .Build(
                    static (builder, tuple) =>
                    {
                        var (key, order) = tuple;

                        return builder
                            .AddStep(new OrderStep.ByMemberStep(key, order))
                            .Build();
                    },
                    (_query.GetKey(projection), order)));

            private OrderBuilder By(Func<GremlinQuery<T1, T2, T3, T4, T5, TFoldedQuery>, IGremlinQueryBase> continuation, Order order) => new(_query
                .Continue()
                .With(continuation)
                .Build(
                    static (builder, traversal, order) => builder
                        .AddStep(new OrderStep.ByTraversalStep(traversal, order))
                        .Build(),
                    order));
        }
    }
}
