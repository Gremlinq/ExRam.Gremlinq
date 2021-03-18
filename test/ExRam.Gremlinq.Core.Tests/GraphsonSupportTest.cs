using System;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Tests
{
    public class GraphsonSupportTest : GremlinqTestBase
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

        private readonly IGremlinQuerySource _g;

        public GraphsonSupportTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _g = g
                .ConfigureEnvironment(env => env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())));
        }

        [Fact]
        public void JToken_Load_does_not_reuse()
        {
            var token = JToken.Parse(GetJson("Single_Language"));

            var readToken1 = JToken.Load(new JTokenReader(token));
            var readToken2 = JToken.Load(new JTokenReader(token));

            readToken1
                .Should()
                .NotBeSameAs(readToken2);
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            await _g
                .WithExecutor(GetJson("Graphson3ReferenceVertex"))
                .V()
                .Verify();
        }

        [Fact]
        public async Task Configured_property_name()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(prop => prop
                            .ConfigureElement<Person>(conf => conf
                                .ConfigureName(x => x.Name, "replacement")))))
                .WithExecutor("[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]")
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task IsDescribedIn()
        {
            await _g
                .WithExecutor(GetJson("Single_WorksFor"))
                .E<WorksFor>()
                .Verify();
        }

        [Fact]
        public async Task DynamicData()
        {
            await _g
                .WithExecutor("{ \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } }")
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("values", __ => __.Values())
                    .By("count", __ => __.Count()))
                .Verify();
        }

        [Fact]
        public async Task WorksFor_with_Graphson3()
        {
            await _g
                .WithExecutor("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"WorksFor\",\"inVLabel\":\"Company\",\"outVLabel\":\"Person\",\"inV\":\"companyId\",\"outV\":\"personId\",\"properties\":{\"Role\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Role\",\"value\":\"Admin\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}")
                .E<WorksFor>()
                .Verify();
        }

        [Fact]
        public async Task Empty1()
        {
            await _g
                .WithExecutor("[]")
                .V()
                .Drop()
                .Verify();
        }

        [Fact]
        public async Task Empty2()
        {
            await _g
                .WithExecutor("[]")
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task String_Ids()
        {
            await _g
                .WithExecutor("[ \"id1\", \"id2\" ]")
                .V()
                .Id()
                .Verify();
        }

        [Fact]
        public async Task String_Ids2()
        {
            await _g
                .WithExecutor("[ \"1\", \"2\" ]")
                .V()
                .Id()
                .Verify();
        }

        [Fact]
        public async Task Int_Ids()
        {
            await _g
                .WithExecutor("[ 1, 2 ]")
                .V()
                .Id()
                .Verify();
        }

        [Fact]
        public async Task Empty_to_ints()
        {
            await _g
                .WithExecutor("[{ \"Item1\": [], \"Item2\": [] }]")
                .V<(int[] ints, string[] strings)>()
                .Verify();   //Must be Verify(...).
        }

        [Fact]
        public async Task Mixed_Ids()
        {
            await _g
                .WithExecutor("[ 1, \"id2\" ]")
                .V()
                .Id()
                .Verify();
        }

        [Fact]
        public async Task DateTime_is_UTC()
        {
            await _g
                .WithExecutor(GetJson("Single_Company"))
                .V<Company>()
                .Verify();
        }

        [Fact]
        public async Task SingleCompany_dynamic()
        {
            await _g
                .WithExecutor(GetJson("Single_Company"))
                .V<dynamic>()
                .Verify();
        }

        [Fact]
        public async Task Language_unknown_type()
        {
            await _g
                .WithExecutor(GetJson("Single_Language"))
                .V<object>()
                .Verify();
        }

        [Fact]
        public async Task Language_unknown_type_without_model()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .WithExecutor(GetJson("Single_Language"))
                .V()
                .Cast<object>()
                .Verify();
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_Language"))
                .V<Language>()
                .Verify();
        }

        [Fact]
        public async Task Language_strongly_typed_without_model()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .WithExecutor(GetJson("Single_Language"))
                .V()
                .Cast<Language>()
                .Verify();
        }

        [Fact]
        public async Task Language_to_generic_vertex()
        {
            await _g
                .WithExecutor(GetJson("Single_Language"))
                .V<Vertex>()
                .Verify();
        }

        [Fact]
        public async Task Languages_to_object()
        {
            await _g
                .WithExecutor(GetJson("Array_of_Languages"))
                .V<object>()
                .Verify();
        }

        [Fact]
        public async Task Person_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_Person"))
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task Person_with_null()
        {
            await _g
                .WithExecutor(GetJson("Single_Person_with_null"))
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task Person_StringId()
        {
            await _g
                .WithExecutor(GetJson("Single_Person_String_Id"))
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task Person_lowercase_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_Person_lowercase_properties"))
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task Person_without_PhoneNumbers_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_Person_without_PhoneNumbers"))
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task TimeFrame_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_TimeFrame"))
                .V<TimeFrame>()
                .Verify();
        }

        [Fact(Skip = "Not standard behaviour!")]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            await _g
                .WithExecutor(GetJson("Single_TimeFrame_with_numbers"))
                .V<TimeFrame>()
                .Verify();
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            await _g
                .WithExecutor(GetJson("Single_Language"))
                .V()
                .Verify();
        }

        [Fact]
        public async Task Tuple()
        {
            await _g
                .WithExecutor(GetJson("Tuple_of_Person_Language"))
                .V()
                .Cast<(Person, Language)>()
                .Verify();
        }

        [Fact]
        public async Task Tuple_vertex_vertex()
        {
            await _g
                .WithExecutor(GetJson("Tuple_of_Person_Language"))
                .V()
                .Cast<(Vertex, Vertex)>()
                .Verify();
        }

        [Fact]
        public async Task NamedTuple()
        {
            await _g
                .WithExecutor(GetJson("Named_tuple_of_Person_Language"))
                .V()
                .Cast<PersonLanguageTuple>()
                .Verify();
        }

        [Fact]
        public async Task Graphson3_Tuple()
        {
            await _g
                .WithExecutor(GetJson("Graphson3_Tuple_of_Person_Language"))
                .V()
                .Cast<(Person, Language)>()
                .Verify();
        }

        [Fact]
        public async Task SingleVertex_as_array()
        {
            await _g
                .WithExecutor("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]")
                .V<Person>()
                .Fold()
                .Verify();
        }

        [Fact]
        public async Task SingleVertex_as_array_of_arrays()
        {
            await _g
                .WithExecutor("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]")
                .V<Person>()
                .Fold()
                .Fold()
                .Verify();
        }

        [Fact]
        public async Task Graphson2Path()
        {
            await _g
                .WithExecutor(GetJson("Graphson2_Paths"))
                .V<Language>()
                .Cast<Core.GraphElements.Path[]>()
                .Verify();
        }

        [Fact]
        public async Task Graphson3Path()
        {
            await _g
                .WithExecutor(GetJson("Graphson3_Paths"))
                .V<Person>()
                .Cast<Core.GraphElements.Path[]>()
                .Verify();
        }

        [Fact]
        public async Task Array()
        {
            await _g
                .WithExecutor(GetJson("Array_of_Languages"))
                .V()
                .Cast<Language[]>()
                .Verify();
        }

        [Fact]
        public async Task Nested_Array()
        {
            await _g
                .WithExecutor(GetJson("Nested_array_of_Languages"))
                .V()
                .Cast<Language[][]>()
                .Verify();
        }

        [Fact]
        public async Task Scalar()
        {
            await _g
                .WithExecutor("[ 36 ]")
                .V()
                .Cast<int>()
                .Verify();
        }

        [Fact]
        public async Task Meta_Properties()
        {
            await _g
                .WithExecutor(GetJson("Country_with_meta_properties"))
                .V<Country>()
                .Verify();
        }

        [Fact]
        public async Task VertexProperties()
        {
            await _g
                .WithExecutor(GetJson("VertexProperties"))
                .V()
                .Properties()
                .Verify();
        }

        [Fact]
        public async Task VertexProperties_with_model()
        {
            await _g
                .WithExecutor(GetJson("VertexProperties"))
                .V()
                .Properties()
                .Meta<MetaPoco>()
                .Verify();
        }

        [Fact]
        public async Task MetaProperties()
        {
            await _g
                .WithExecutor(GetJson("Properties"))
                .V()
                .Properties()
                .Properties()
                .Verify();
        }

        [Fact]
        public async Task Guid()
        {
            await _g
                .WithExecutor("[ \"FCE0765A-454F-4D00-83DA-D76790156E29\" ]")
                .V<Guid>()
                .Verify();
        }

        [Fact]
        public async Task VertexPropertyWithoutProperties()
        {
            await _g
                .WithExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]")
                .V<Person>()
                .Properties(x => x.SomeObscureProperty!)
                .Verify();
        }

        [Fact]
        public async Task VertexPropertyWithDateTimeOffset()
        {
            await _g
                .WithExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]")
                .V<Person>()
                .Properties(x => x.Name!)
                .Verify();
        }

        [Fact]
        public async Task PropertyWithDateTimeOffset()
        {
            await _g
                .WithExecutor("{ \"@type\": \"g:List\",\"@value\": [ { \"@type\": \"g:Property\", \"@value\": { \"key\": \"ValidFrom\", \"value\": { \"@type\": \"g:Date\", \"@value\": 1548169812555 } } } ] }")
                .V<Person>()
                .Properties(x => x.Name!)
                .Properties(x => x.ValidFrom)
                .Verify();
        }

        [Fact]
        public async Task BulkSet()
        {
            await _g
                .WithExecutor("{ \"@type\": \"g:BulkSet\", \"@value\": [ { \"@type\": \"g:Vertex\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 69 }, \"label\": \"Person\" } }, { \"@type\": \"g:Int64\", \"@value\": 1 } ] }")
                .V<Person>()
                .Verify();
        }

        [Fact]
        public async Task Traverser()
        {
            await _g
                .WithExecutor(GetJson("Traverser"))
                .V<Company>()
                .Verify();
        }

        [Fact]
        public async Task Nullable()
        {
            await _g
                .WithExecutor("[ { \"Item1\": [],  \"Item2\": [], \"Item3\": \"someString\", \"Item4\": \"someString\", \"Item5\": [],  \"Item5\": null } ]")
                .V<(string, string?, string, string?, int?, int?)>()
                .Verify();
        }

        private static string GetJson(string name)
        {
            return new StreamReader(File.OpenRead($"..\\..\\..\\..\\..\\files\\GraphSon\\{name}.json")).ReadToEnd();
        }
    }
}
