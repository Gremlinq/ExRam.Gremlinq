using ExRam.Gremlinq.Core.GraphElements;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TypeSystem
    {
        private sealed class Vertex
        {
            public string String { get; }
            public string[] Strings { get; }

            public VertexProperty<string> StringVertexProperty { get; }
            public VertexProperty<string>[] StringVertexProperties { get; }

            public VertexProperty<string, object> MetaStringVertexProperty { get; }
            public VertexProperty<string, object>[] MetaStringVertexProperties { get; }
        }

        private sealed class Edge
        {
            public string String { get; }
            public string[] Strings { get; }

            public Property<string> StringEdgeProperty { get; }
            public Property<string>[] StringEdgeProperties { get; }
        }

        [Fact]
        public void V_Properties_String()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_Strings()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings)
                .Where(x => x.Value == "hallo")
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_MetaStringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_MetaStringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_String_Strings()
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
        public void V_Properties_String_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_Strings_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_String_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Properties_Strings_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Values_String()
        {
            g
                .V<Vertex>()
                .Values(x => x.String)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_Strings()
        {
            g
                .V<Vertex>()
                .Values(x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Values(x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Values(x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_MetaStringVertexProperty()
        {
            g
                .V<Vertex>()
                .Values(x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_MetaStringVertexProperties()
        {
            g
                .V<Vertex>()
                .Values(x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }
        
        [Fact]
        public void V_Values_String_Strings()
        {
            g
                .V<Vertex>()
                .Values(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();

            g
                .V<Vertex>()
                .Values<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_String_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Values(x => x.String, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_Strings_StringVertexProperty()
        {
            g
                .V<Vertex>()
                .Values(x => x.Strings, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_String_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Values(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }

        [Fact]
        public void V_Values_Strings_StringVertexProperties()
        {
            g
                .V<Vertex>()
                .Values(x => x.Strings, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }































        [Fact]
        public void E_Properties_String()
        {
            g
                .E<Edge>()
                .Properties(x => x.String)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_Strings()
        {
            g
                .E<Edge>()
                .Properties(x => x.Strings)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Properties(x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Properties(x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_String_Strings()
        {
            g
                .E<Edge>()
                .Properties(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<object>, object>>();

            g
                .E<Edge>()
                .Properties<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_String_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Properties(x => x.String, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_Strings_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Properties(x => x.Strings, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<string>, string>>();
        }

        [Fact]
        public void E_Properties_String_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Properties(x => x.String, x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<object>, object>>();
        }

        [Fact]
        public void E_Properties_Strings_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Properties(x => x.Strings, x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IEdgePropertyGremlinQuery<Property<object>, object>>();
        }

        [Fact]
        public void E_Values_String()
        {
            g
                .E<Edge>()
                .Values(x => x.String)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_Strings()
        {
            g
                .E<Edge>()
                .Values(x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Values(x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Values(x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_String_Strings()
        {
            g
                .E<Edge>()
                .Values(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();

            g
                .E<Edge>()
                .Values<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_String_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Values(x => x.String, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_Strings_StringEdgeProperty()
        {
            g
                .E<Edge>()
                .Values(x => x.Strings, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_String_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Values(x => x.String, x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }

        [Fact]
        public void E_Values_Strings_StringEdgeProperties()
        {
            g
                .E<Edge>()
                .Values(x => x.Strings, x => x.StringEdgeProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }
    }
}
