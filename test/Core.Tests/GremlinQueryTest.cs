using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryTest : VerifyBase
    {
        private readonly IGremlinQuerySource _g;

        public GremlinQueryTest() : base()
        {
            _g = g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public void ChangeQueryType_to_IGremlinQuerySource()
        {
            _g
                .V()
                .AsAdmin()
                .ChangeQueryType<IGremlinQuerySource>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void AddPartitionStrategy_To_IGremlinQuerySource()
        {
            var query = _g
                .WithPartitionStrategy("partitionKey");

            query.Should()
                .BeAssignableTo<IGremlinQuerySource>()
                .Which
                .AsAdmin()
                .Steps
                .Steps
                .IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void ChangeQueryType_optimizes()
        {
            var query = _g
                .V<Person>();

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);

            query.AsAdmin().ChangeQueryType<IEdgeGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);
        }

        [Fact]
        public void ChangeQueryType_takes_array_element_types_into_account()
        {
            var query = _g
                .V<Person>();

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase<Person[]>>()
                .Should()
                .BeAssignableTo<IArrayGremlinQueryBase<Person>>();
        }

        [Fact]
        public async Task ForceVertex_has_correct_semantics()
        {
            await Verify(_g
                .V<Person>()
                .Count()
                .ForceVertex()
                .ToTraversal()
                .Projection);
        }

        [Fact]
        public void Lower_chain_from_value() => _g
            .Inject(0)
            .Lower();

        [Fact]
        public void Lower_chain_from_untyped_vertex() => _g
            .V()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_typed_vertex() => _g
            .V<Person>()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_untyped_edge() => _g
            .E()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_typed_edge() => _g
            .E<WorksFor>()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_array() => _g
            .Inject(0)
            .Fold()
            .Lower();
    }
}
