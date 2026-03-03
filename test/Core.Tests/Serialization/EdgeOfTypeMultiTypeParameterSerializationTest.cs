using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class EdgeOfTypeMultiTypeParameterSerializationTest : GremlinqTestBase, IClassFixture<GremlinqFixture>
    {
        public class NestedEdge;

        public class Edge1 : NestedEdge;
        public class Edge2 : NestedEdge;
        public class Edge3 : NestedEdge;
        public class Edge4 : NestedEdge;
        public class Edge5 : NestedEdge;
        public class Edge6 : NestedEdge;
        public class Edge7 : NestedEdge;
        public class Edge8 : NestedEdge;
        public class Edge9 : NestedEdge;
        public class Edge10 : NestedEdge;
        public class Edge11 : NestedEdge;
        public class Edge12 : NestedEdge;
        public class Edge13 : NestedEdge;
        public class Edge14 : NestedEdge;
        public class Edge15 : NestedEdge;
        public class Edge16 : NestedEdge;

        private readonly IGremlinQuerySource _g;

        public EdgeOfTypeMultiTypeParameterSerializationTest(GremlinqFixture fixture) : base(new SerializingVerifier<Bytecode>())
        {
            _g = fixture
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, NestedEdge>()));
        }

        [Fact]
        public virtual Task E_without_type() => _g
            .E()
            .Verify();

        [Fact]
        public virtual Task E_with_object_type() => _g
            .E<object>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_object_type() => _g
            .E<NestedEdge>()
            .OfType<object>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_one_type() => _g
            .E<NestedEdge>()
            .OfType<Edge1>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_two_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_three_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_four_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_five_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_six_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_seven_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_eight_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_nine_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_ten_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_eleven_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_twelve_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11, Edge12>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_thirteen_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11, Edge12, Edge13>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_fourteen_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11, Edge12, Edge13, Edge14>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_fifteen_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11, Edge12, Edge13, Edge14, Edge15>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_sixteen_types() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge4, Edge5, Edge6, Edge7, Edge8, Edge9, Edge10, Edge11, Edge12, Edge13, Edge14, Edge15, Edge16>()
            .Verify();

        [Fact]
        public virtual Task Labels_are_distinct() => _g
            .E<NestedEdge>()
            .OfType<Edge1, Edge2, Edge3, Edge1, Edge2, Edge3, Edge4, Edge5, Edge5, Edge6>()
            .Verify();
    }
}
