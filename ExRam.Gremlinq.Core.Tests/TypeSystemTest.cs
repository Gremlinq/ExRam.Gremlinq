using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.Kernel;
using ExRam.Gremlinq.Core.GraphElements;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TypeSystemTest : VerifyBase
    {
        private sealed class Vertex
        {
            public string String { get; }
            public string[] Strings { get; }

            public VertexProperty<string> StringVertexProperty { get; }
            public VertexProperty<string>[] StringVertexProperties { get; }

            public VertexProperty<int> IntVertexProperty { get; }
            public VertexProperty<int>[] IntVertexProperties { get; }

            public VertexProperty<string, object> MetaStringVertexProperty { get; }
            public VertexProperty<string, object>[] MetaStringVertexProperties { get; }

            public VertexProperty<object, object> MetaObjectVertexProperty { get; }
            public VertexProperty<object, object>[] MetaObjectVertexProperties { get; }
        }

        private sealed class Edge
        {
            public string String { get; }
            
            public Property<string> StringEdgeProperty { get; }
        }

        private sealed class SpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var type = default(Type);

                if (request is ParameterInfo parameter)
                    type = parameter.ParameterType;
                else if (request is Type typeRequest)
                    type = typeRequest;

                if (type == typeof(ImmutableArray<object>))
                    return new object[] { context.Resolve(typeof(string)) }.ToImmutableArray();

                if (type == typeof(ImmutableArray<string>))
                    return new string[] { (string)context.Resolve(typeof(string)) }.ToImmutableArray();

                if (type == typeof(ImmutableArray<Traversal>))
                    return new Traversal[] { (Traversal)context.Resolve(typeof(Traversal)) }.ToImmutableArray();

                if (type == typeof(ImmutableArray<StepLabel>))
                    return new StepLabel[] { (StepLabel)context.Resolve(typeof(StepLabel)) }.ToImmutableArray();

                if (type == typeof(ImmutableArray<Type>))
                    return new Type[] { typeof(SubgraphStrategy) }.ToImmutableArray();

                if (type == typeof(Traversal))
                    return (Traversal)IdentityStep.Instance;

                if (type == typeof(ILambda))
                    return Lambda.Groovy("lambda");

                if (type == typeof(string))
                    return "string";

                if (type == typeof(double))
                    return 47.11;

                if (type == typeof(int))
                    return 4711;

                if (type == typeof(long))
                    return 4711;

                return new NoSpecimen();
            }
        }

        public static readonly Step[] AllSteps;

        private IGremlinQuerySource _g = g.ConfigureEnvironment(_ => _);

        static TypeSystemTest()
        {
            var fixture = new Fixture();
            var stepTypes = typeof(Step).Assembly.DefinedTypes
                .Where(type => typeof(Step).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract);


            fixture.Customizations.Add(new SpecimenBuilder());
            fixture.Customizations.Add(new TypeRelay(
                typeof(StepLabel),
                typeof(StepLabel<object>)));

            AllSteps = stepTypes
                .Select(type => (Step)fixture.Create(new SeededRequest(type, 4711), new SpecimenContext(fixture)))
                .ToArray();
        }

        public TypeSystemTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task All_Steps_can_be_created()
        {
            await Verify(AllSteps.Select(step => (step.GetType(), step)));
        }

        [Fact]
        public void V_Properties_String()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_Strings()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_IntVertexProperty_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.IntVertexProperty, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Properties_String_IntVertexProperty_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.IntVertexProperty, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Properties_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_String_MetaStringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_Strings_MetaStringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_String_Strings_MetaStringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.Strings, x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_MetaStringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_MetaObjectVertexProperty_MetaStringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.MetaObjectVertexProperty, x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object, object>, object, object>>();
        }

        [Fact]
        public void V_Properties_MetaStringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string, object>, string, object>>();
        }

        [Fact]
        public void V_Properties_String_Strings()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();

            _g
                .V<Vertex>()
                .Properties<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_String_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_Strings_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_String_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Properties_String_StringVertexProperties_Explicit()
        {
            _g
                .V<Vertex>()
                .Properties<string>(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<string>, string>>();
        }

        [Fact]
        public void V_Properties_Strings_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Properties(x => x.Strings, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IVertexPropertyGremlinQuery<VertexProperty<object>, object>>();
        }

        [Fact]
        public void V_Values_String()
        {
            _g
                .V<Vertex>()
                .Values(x => x.String)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_Strings()
        {
            _g
                .V<Vertex>()
                .Values(x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Values(x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Values(x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_MetaStringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Values(x => x.MetaStringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_MetaStringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Values(x => x.MetaStringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }
        
        [Fact]
        public void V_Values_String_Strings()
        {
            _g
                .V<Vertex>()
                .Values(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();

            _g
                .V<Vertex>()
                .Values<string>(x => x.String, x => x.Strings)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_String_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Values(x => x.String, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_Strings_StringVertexProperty()
        {
            _g
                .V<Vertex>()
                .Values(x => x.Strings, x => x.StringVertexProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void V_Values_String_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Values(x => x.String, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }

        [Fact]
        public void V_Values_Strings_StringVertexProperties()
        {
            _g
                .V<Vertex>()
                .Values(x => x.Strings, x => x.StringVertexProperties)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<object>>();
        }
        
        [Fact]
        public void E_Properties_String()
        {
            _g
                .E<Edge>()
                .Properties(x => x.String)
                .Should()
                .BeAssignableTo<IPropertyGremlinQuery<Property<string>>>();
        }

        [Fact]
        public void E_Properties_StringEdgeProperty()
        {
            _g
                .E<Edge>()
                .Properties(x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IPropertyGremlinQuery<Property<string>>>();
        }

        [Fact]
        public void E_Properties_String_StringEdgeProperty()
        {
            _g
                .E<Edge>()
                .Properties(x => x.String, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IPropertyGremlinQuery<Property<string>>>();
        }

        [Fact]
        public void E_Values_String()
        {
            _g
                .E<Edge>()
                .Values(x => x.String)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_StringEdgeProperty()
        {
            _g
                .E<Edge>()
                .Values(x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void E_Values_String_StringEdgeProperty()
        {
            _g
                .E<Edge>()
                .Values(x => x.String, x => x.StringEdgeProperty)
                .Should()
                .BeAssignableTo<IValueGremlinQuery<string>>();
        }

        [Fact]
        public void Project_16Tuple()
        {
            _g
                .V<Vertex>()
                .Project(p => p
                    .ToTuple()
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String))
                    .By(__ => __.Values(x => x.String)))
                .Should()
                .BeAssignableTo<IValueGremlinQuery<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>>();
        }
    }
}
