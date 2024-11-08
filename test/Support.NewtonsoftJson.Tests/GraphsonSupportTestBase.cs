using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class GraphsonSupportTestBase<TNativeToken> : VerifyBase
    {
        private sealed class MetaPoco
        {
            public string? MetaKey { get; set; }
        }

        private sealed class PersonLanguageTuple
        {
            public Person? Key { get; set; }
            public Language? Value { get; set; }
        }

        protected GraphsonSupportTestBase(IGremlinQueryEnvironment environment) : base()
        {
            Environment = environment;
        }

        protected Task Verify<T>(string token, IGremlinQueryEnvironment environment) => Verify(environment
            .Deserializer
            .TransformTo<T[]>()
            .From(CreateNativeToken(token), environment));

        protected Task Verify<T>(string token) => Verify<T>(token, Environment);

        protected abstract TNativeToken CreateNativeToken(string str);

        protected IGremlinQueryEnvironment Environment { get; }

        [Fact]
        public Task GraphSon3ReferenceVertex() => Verify<object>(GetJson("Graphson3ReferenceVertex"));

        [Fact]
        public Task Configured_property_name() => Verify<Person>(
            "[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]",
            Environment
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement")))));

        [Fact]
        public Task IsDescribedIn() => Verify<WorksFor>(GetJson("Single_WorksFor"));

        [Fact]
        public Task DynamicData() => Verify<dynamic>("[ { \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } } ]");

        [Fact]
        public Task Empty1() => Verify<object>("[]");

        [Fact]
        public Task Empty2() => Verify<Person>("[]");

        [Fact]
        public Task String_Ids() => Verify<object>("[ \"id1\", \"id2\" ]");

        [Fact]
        public Task String_Ids2() => Verify<object>("[ \"1\", \"2\" ]");

        [Fact]
        public Task Int_Ids() => Verify<object>("[ 1, 2 ]");

        [Fact]
        public Task Empty_to_ints() => Verify<(int[] ints, string[] strings)>("[{ \"Item1\": [], \"Item2\": [] }]");

        [Fact]
        public Task Mixed_Ids() => Verify<object>("[ 1, \"id2\" ]");

        [Fact]
        public Task DateTime_is_UTC() => Verify<Company>(GetJson("Single_Company"));

        [Fact]
        public Task Language_unknown_type() => Verify<object>(GetJson("Single_Language"));

        [Fact]
        public Task Language_strongly_typed() => Verify<Language>(GetJson("Single_Language"));

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
        public Task Graphson2Path() => Verify<Path>(GetJson("Graphson2_Paths"));

        [Fact]
        public Task Graphson3Path() => Verify<Path>(GetJson("Graphson3_Paths"));

        [Fact]
        public Task Array() => Verify<Language[]>(GetJson("Array_of_Languages"));

        [Fact]
        public Task Nested_Array() => Verify<Language[][]>(GetJson("Nested_array_of_Languages"));

        [Fact]
        public Task Scalar() => Verify<int>("[ 36 ]");

        [Fact]
        public Task Meta_Properties() => Verify<Country>(GetJson("Country_with_meta_properties"));

        [Fact]
        public Task VertexProperties() => Verify<VertexProperty<object>>(GetJson("VertexProperties"));

        [Fact]
        public Task VertexProperties_with_model() => Verify<VertexProperty<object, MetaPoco>>(GetJson("VertexProperties"));

        [Fact]
        public Task MetaProperties() => Verify<Property<object>>(GetJson("Properties"));

        [Fact]
        public Task Guid() => Verify<Guid>("[ \"FCE0765A-454F-4D00-83DA-D76790156E29\" ]");

        [Fact]
        public Task Nullable() => Verify<int?>("[ 42 ]");

        [Fact]
        public Task Nullable_null() => Verify<int?>("[ 42, null ]");

        [Fact]
        public Task VertexPropertyWithoutProperties() => Verify<VertexProperty<object, object>>("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]");

        [Fact]
        public Task VertexPropertyWithDateTimeOffset() => Verify<VertexProperty<string, PropertyValidity>>("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]");
        
        protected static string GetJson(string name) => new StreamReader(File.OpenRead($"../../../../files/GraphSon/{name}.json")).ReadToEnd();
    }
}
