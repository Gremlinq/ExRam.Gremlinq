using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue2471 : GremlinqTestBase
    {
        private readonly IGremlinQuerySource _g;

        public Issue2471() : base(new DebugGremlinQueryVerifier())
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public Task Repro() => _g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Limit(1))
            .Verify();

        [Fact]
        public Task Out_Limit() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Limit(1)));

        [Fact]
        public Task Out_Range() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Range(1, 3)));

        [Fact]
        public Task Out_Skip() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Skip(1)));

        [Fact]
        public Task Out_Tail() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Tail(1)));

        [Fact]
        public Task Out_Dedup() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Dedup()));

        [Fact]
        public Task Out_Order() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .OfType<SiblingVertex>()
                .Order(b => b
                    .By(x => x.FoundingDate))));

        [Fact]
        public Task Out_Fold() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Fold()));

        [Fact]
        public Task Out_Count() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Count()));

        [Fact]
        public Task Out_Limit_followed_by_step() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .Limit(1))
            .In<RichEdge>());

        [Fact]
        public Task Coalesce_Out_Limit() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Coalesce(__ => __
                    .Out<RichEdge>()
                    .Limit(1))));

        [Fact]
        public Task Drop() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Drop()));

        [Fact]
        public Task Out_with_projection() => _g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>())
            .Verify();

        [Fact]
        public Task Out() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()));

        [Fact]
        public Task OfType_after_OfType() => VerifySteps(_g
            .V<AbstractVertex>()
            .Coalesce(__ => __
                .OfType<RichVertex>()));

        [Fact]
        public Task Where_after_Where() => VerifySteps(_g
            .V<RichVertex>()
            .Where(x => x.Age > 36)
            .Coalesce(__ => __
                .Where(x => x.Age < 42)));

        [Fact]
        public Task Out_Values() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Out<RichEdge>()
                .OfType<SiblingVertex>()
                .Values(x => x.FoundingDate)));

        [Fact]
        public Task Map_Out_Limit() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Map(__ => __
                    .Out<RichEdge>()
                    .Limit(1))));

        [Fact]
        public Task Fold_CountLocal() => VerifySteps(_g
            .V<RichVertex>()
            .Fold()
            .Coalesce(__ => __
                .CountLocal()));

        [Fact]
        public Task Identity() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(__ => __
                .Identity()));

        [Fact]
        public Task Out_Limit_and_In() => VerifySteps(_g
            .V<RichVertex>()
            .Coalesce(
                __ => __
                    .Out<RichEdge>()
                    .Limit(1),
                __ => __
                    .In<RichEdge>()));

        private static Task VerifySteps(IGremlinQueryBase query) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<object>>(
                static traversal => traversal,
                static _ => Projection.Empty)
            .Verify();
    }
}
