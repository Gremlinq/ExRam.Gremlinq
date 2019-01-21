using ExRam.Gremlinq.Core.GraphElements;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TypeSystem
    {
        private sealed class Vertex : IVertex
        {
            public string String { get; }
            public string[] Strings { get; }

            public VertexProperty<string> StringVertexProperty { get; }
            public VertexProperty<string>[] StringVertexProperties { get; }

            public VertexProperty<string, object> MetaStringVertexProperty { get; }
            public VertexProperty<string, object>[] MetaStringVertexProperties { get; }

            public object Id { get; set; }
        }

        [Fact]
        public void Properties_String()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_Strings()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void MetaStringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void Properties_MetaStringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }


        [Fact]
        public void Properties_String_Strings()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();

            g
                .V<Vertex>()
                .Properties<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_String_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_Strings_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void Properties_String_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void Properties_Strings_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }
    }
}
