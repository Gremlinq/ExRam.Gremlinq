using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class VertexOfTypeMultiTypeParameterSerializationTest : GremlinqTestBase, IClassFixture<GremlinqFixture>
    {
        public class NestedVertex;

        public class Vertex1 : NestedVertex;
        public class Vertex2 : NestedVertex;
        public class Vertex3 : NestedVertex;
        public class Vertex4 : NestedVertex;
        public class Vertex5 : NestedVertex;
        public class Vertex6 : NestedVertex;
        public class Vertex7 : NestedVertex;
        public class Vertex8 : NestedVertex;
        public class Vertex9 : NestedVertex;
        public class Vertex10 : NestedVertex;
        public class Vertex11 : NestedVertex;
        public class Vertex12 : NestedVertex;
        public class Vertex13 : NestedVertex;
        public class Vertex14 : NestedVertex;
        public class Vertex15 : NestedVertex;
        public class Vertex16 : NestedVertex;

        private readonly IGremlinQuerySource _g;

        public VertexOfTypeMultiTypeParameterSerializationTest(GremlinqFixture fixture) : base(new SerializingVerifier<Bytecode>())
        {
            _g = fixture
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<NestedVertex, Edge>()));
        }

        [Fact]
        public virtual Task OfType_with_object_type() => _g
            .V<NestedVertex>()
            .OfType<object>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_one_type() => _g
            .V<NestedVertex>()
            .OfType<Vertex1>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_two_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_three_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_four_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_five_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_six_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_seven_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_eight_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_nine_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_ten_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_eleven_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_twelve_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11, Vertex12>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_thirteen_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11, Vertex12, Vertex13>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_fourteen_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11, Vertex12, Vertex13, Vertex14>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_fifteen_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11, Vertex12, Vertex13, Vertex14, Vertex15>()
            .Verify();

        [Fact]
        public virtual Task OfType_with_sixteen_types() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex6, Vertex7, Vertex8, Vertex9, Vertex10, Vertex11, Vertex12, Vertex13, Vertex14, Vertex15, Vertex16>()
            .Verify();

        [Fact]
        public virtual Task Labels_are_distinct() => _g
            .V<NestedVertex>()
            .OfType<Vertex1, Vertex2, Vertex3, Vertex1, Vertex2, Vertex3, Vertex4, Vertex5, Vertex5, Vertex6>()
            .Verify();
    }
}
