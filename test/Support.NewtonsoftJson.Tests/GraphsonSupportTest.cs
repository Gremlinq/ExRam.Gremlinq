using FluentAssertions;
using ExRam.Gremlinq.Tests.Entities;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core.GraphElements;
using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public sealed class GraphsonSupportTest : VerifyBase
    {
        private readonly struct NativeType
        {
            public NativeType(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        private sealed class MetaPoco
        {
            public string? MetaKey { get; set; }
        }

        private sealed class PersonLanguageTuple
        {
            public Person? Key { get; set; }
            public Language? Value { get; set; }
        }

        private readonly IGremlinQueryEnvironment _environment;

        public GraphsonSupportTest() : base()
        {
            _environment = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .UseNewtonsoftJson())
                .AsAdmin()
                .Environment;
        }

        private Task Verify<T>(JToken token, IGremlinQueryEnvironment environment)
        {
            return Verify(environment
                .Deserializer
                .TransformTo<T[]>()
                .From(token, environment));
        }

        private Task Verify<T>(JToken token) => Verify<T>(token, _environment);

        [Fact]
        public void JToken_Load_does_not_reuse()
        {
            var token = GetJson("Single_Language");

            var readToken1 = JToken.Load(new JTokenReader(token));
            var readToken2 = JToken.Load(new JTokenReader(token));

            readToken1
                .Should()
                .NotBeSameAs(readToken2);
        }

        [Fact]
        public Task GraphSon3ReferenceVertex() => Verify<object>(GetJson("Graphson3ReferenceVertex"));

        [Fact]
        public Task Configured_property_name() => Verify<Person>(
            JToken.Parse("[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]"),
            _environment
                .ConfigureModel(model => model
                    .ConfigureProperties(prop => prop
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement")))));

        [Fact]
        public Task IsDescribedIn() => Verify<WorksFor>(GetJson("Single_WorksFor"));

        [Fact]
        public Task DynamicData() => Verify<dynamic>(JToken.Parse("{ \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } }"));

        [Fact]
        public Task Empty1() => Verify<object>(JToken.Parse("[]"));

        [Fact]
        public Task Empty2() => Verify<Person>(JToken.Parse("[]"));

        [Fact]
        public Task String_Ids() => Verify<object>(JToken.Parse("[ \"id1\", \"id2\" ]"));

        [Fact]
        public Task String_Ids2() => Verify<object>(JToken.Parse("[ \"1\", \"2\" ]"));

        [Fact]
        public Task Int_Ids() => Verify<object>(JToken.Parse("[ 1, 2 ]"));

        [Fact]
        public Task Empty_to_ints() => Verify<(int[] ints, string[] strings)>(JToken.Parse("[{ \"Item1\": [], \"Item2\": [] }]"));

        [Fact]
        public Task Mixed_Ids() => Verify<object>(JToken.Parse("[ 1, \"id2\" ]"));

        [Fact]
        public Task DateTime_is_UTC() => Verify<Company>(GetJson("Single_Company"));

        [Fact]
        public Task Language_unknown_type() => Verify<object>(GetJson("Single_Language"));

        [Fact]
        public Task Language_unknown_type_without_model() => Verify<object>(
            GetJson("Single_Language"),
            _environment
                .UseModel(GraphModel.Empty));

        [Fact]
        public Task Language_strongly_typed() => Verify<Language>(GetJson("Single_Language"));

        [Fact]
        public Task Language_strongly_typed_without_model() => Verify<Language>(
            GetJson("Single_Language"),
            _environment
                .UseModel(GraphModel.Empty));

        [Fact]
        public Task Language_to_generic_vertex() => Verify<Vertex>(GetJson("Single_Language"));

        [Fact]
        public Task Languages_to_object() => Verify<object>(GetJson("Array_of_Languages"));

        [Fact]
        public Task Person_strongly_typed() => Verify<Person>(GetJson("Single_Person"));

        [Fact]
        public Task Person_with_null() => Verify<Person>(GetJson("Single_Person_with_null"));

        [Fact]
        public Task Person_StringId() => Verify<Person>(GetJson("Single_Person_String_Id"));

        [Fact]
        public Task Person_lowercase_strongly_typed() => Verify<Person>(GetJson("Single_Person_lowercase_properties"));

        [Fact]
        public Task Person_without_PhoneNumbers_strongly_typed() => Verify<Person>(GetJson("Single_Person_without_PhoneNumbers"));

        [Fact]
        public Task TimeFrame_strongly_typed() => Verify<TimeFrame>(GetJson("Single_TimeFrame"));

        [Fact]
        public Task Language_by_vertex_inheritance() => Verify<object>(GetJson("Single_Language"));

        [Fact]
        public Task Tuple() => Verify<(Person, Language)>(GetJson("Tuple_of_Person_Language"));

        [Fact]
        public Task Tuple_vertex_vertex() => Verify<(Vertex, Vertex)>(GetJson("Tuple_of_Person_Language"));

        [Fact]
        public Task NamedTuple() => Verify<PersonLanguageTuple>(GetJson("Named_tuple_of_Person_Language"));

        [Fact]
        public Task SingleVertex_as_array() => Verify<Person[]>(JToken.Parse("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]"));

        [Fact]
        public Task SingleVertex_as_array_of_arrays() => Verify<Person[][]>(JToken.Parse("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]"));

        [Fact]
        public Task Graphson2Path() => Verify<Path[]>(GetJson("Graphson2_Paths"));

        [Fact]
        public Task Graphson3Path() => Verify<Path[]>(GetJson("Graphson3_Paths"));

        [Fact]
        public Task LargeGraphson3Path() => Verify<Path[]>(GetJson("Large_Graphson3_Paths"));

        [Fact]
        public Task Array() => Verify<Language[]>(GetJson("Array_of_Languages"));

        [Fact]
        public Task Nested_Array() => Verify<Language[][]>(GetJson("Nested_array_of_Languages"));

        [Fact]
        public Task Scalar() => Verify<int>(JToken.Parse("[ 36 ]"));

        [Fact]
        public Task Meta_Properties() => Verify<Country>(GetJson("Country_with_meta_properties"));

        [Fact]
        public Task VertexProperties() => Verify<VertexProperty<object>>(GetJson("VertexProperties"));

        [Fact]
        public Task VertexProperties_with_model() => Verify<VertexProperty<object, MetaPoco>>(GetJson("VertexProperties"));

        [Fact]
        public Task MetaProperties() => Verify<Property<object>>(GetJson("Properties"));

        [Fact]
        public Task Guid() => Verify<Guid>(JToken.Parse("[ \"FCE0765A-454F-4D00-83DA-D76790156E29\" ]"));

        [Fact]
        public Task Nullable() => Verify<int?>(JToken.Parse("[ 42 ]"));

        [Fact]
        public Task Nullable_null() => Verify<int?>(JToken.Parse("[ 42, null ]"));

        [Fact]
        public Task VertexPropertyWithoutProperties() => Verify<VertexProperty<object, object>>(JToken.Parse("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]"));

        [Fact]
        public Task VertexPropertyWithDateTimeOffset() => Verify<VertexProperty<string, PropertyValidity>>(JToken.Parse("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]"));

        [Fact]
        public async Task NativeType_is_deserialized()
        {
            var data = JToken.Parse("[ 42 ]");

            await Verify<NativeType>(data, _environment
                .RegisterNativeType(
                    (nativeType, env, recurse) => 42,
                    (jValue, env, recurse) => jValue.Type is JTokenType.Integer
                        ? new NativeType(jValue.Value<int>())
                        : default));
        }

        [Fact]
        public async Task NativeType_is_only_deserialized_when_requested_explicitly()
        {
            var data = JToken.Parse("[ \"originalString\" ]");

            await Verify<object>(data, _environment
                .RegisterNativeType(
                    (nativeType, env, recurse) => 42,
                    (jValue, env, recurse) => jValue.Type is JTokenType.Integer
                        ? new NativeType(jValue.Value<int>())
                        : default));
        }

        private static JToken GetJson(string name)
        {
            return JToken.Parse(new StreamReader(File.OpenRead($"../../../../files/GraphSon/{name}.json")).ReadToEnd());
        }
    }
}
