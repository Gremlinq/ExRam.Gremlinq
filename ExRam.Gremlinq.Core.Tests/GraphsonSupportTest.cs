using FluentAssertions;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using Newtonsoft.Json.Linq;
using VerifyTests;
using VerifyXunit;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Tests
{
    [UsesVerify]
    public class GraphsonSupportTest
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

        private static readonly string SinglePersonJson;
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleCompanyJson;
        private static readonly string SinglePersonStringId;
        private static readonly string SingleLanguageJson;
        private static readonly string SingleWorksFor;
        private static readonly string SingleTimeFrameJson;
        private static readonly string SinglePersonWithNullJson;
        private static readonly string TupleOfPersonLanguageJson;
        private static readonly string Graphson3ReferenceVertex;
        private static readonly string ThreeCompaniesAsTraverser;
        private static readonly string CountryWithMetaProperties;
        private static readonly string NestedArrayOfLanguagesJson;
        private static readonly string NamedTupleOfPersonLanguageJson;
        private static readonly string SingleTimeFrameWithNumbersJson;
        private static readonly string SinglePersonWithoutPhoneNumbersJson;
        private static readonly string SinglePersonLowercasePropertiesJson;
        private static readonly string Graphson3TupleOfPersonLanguageJson;

        private readonly IGremlinQuerySource _g;

        static GraphsonSupportTest()
        {
            SingleLanguageJson = GetJson("Single_Language");
            SingleCompanyJson = GetJson("Single_Company");
            ThreeCompaniesAsTraverser = GetJson("Traverser");
            SinglePersonJson = GetJson("Single_Person");
            SinglePersonWithNullJson = GetJson("Single_Person_with_null");
            SinglePersonLowercasePropertiesJson = GetJson("Single_Person_lowercase_properties");
            SinglePersonWithoutPhoneNumbersJson = GetJson("Single_Person_without_PhoneNumbers");
            TupleOfPersonLanguageJson = GetJson("Tuple_of_Person_Language");
            NamedTupleOfPersonLanguageJson = GetJson("Named_tuple_of_Person_Language");
            ArrayOfLanguages = GetJson("Array_of_Languages");
            NestedArrayOfLanguagesJson = GetJson("Nested_array_of_Languages");
            SingleTimeFrameJson = GetJson("Single_TimeFrame");
            SingleTimeFrameWithNumbersJson = GetJson("Single_TimeFrame_with_numbers");
            SingleWorksFor = GetJson("Single_WorksFor");
            Graphson3TupleOfPersonLanguageJson = GetJson("Graphson3_Tuple_of_Person_Language");
            Graphson3ReferenceVertex = GetJson("Graphson3ReferenceVertex");
            CountryWithMetaProperties = GetJson("Country_with_meta_properties");
            SinglePersonStringId = GetJson("Single_Person_String_Id");
        }

        public GraphsonSupportTest()
        {
            _g = g
                .ConfigureEnvironment(env => env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())));
        }

        [Fact]
        public void JToken_Load_does_not_reuse()
        {
            var token = JToken.Parse(SingleLanguageJson);

            var readToken1 = JToken.Load(new JTokenReader(token));
            var readToken2 = JToken.Load(new JTokenReader(token));

            readToken1
                .Should()
                .NotBeSameAs(readToken2);
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            await Verify(await _g
                .WithExecutor(Graphson3ReferenceVertex)
                .V()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Configured_property_name()
        {
            await Verify(await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(prop => prop
                            .ConfigureElement<Person>(conf => conf
                                .ConfigureName(x => x.Name, "replacement")))))
                .WithExecutor("[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]")
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task IsDescribedIn()
        {
            await Verify(await _g
                .WithExecutor(SingleWorksFor)
                .E<WorksFor>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task DynamicData()
        {
            await Verify(await _g
                .WithExecutor(SingleWorksFor)
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In()))
                .ToArrayAsync());
        }

        [Fact]
        public async Task WorksFor_with_Graphson3()
        {
            await Verify(await _g
                .WithExecutor("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"WorksFor\",\"inVLabel\":\"Company\",\"outVLabel\":\"Person\",\"inV\":\"companyId\",\"outV\":\"personId\",\"properties\":{\"Role\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Role\",\"value\":\"Admin\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}")
                .E<WorksFor>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Empty1()
        {
            await Verify(await _g
                .WithExecutor("[]")
                .V()
                .Drop()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Empty2()
        {
            await Verify(await _g
                .WithExecutor("[]")
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task String_Ids()
        {
            await Verify(await _g
                .WithExecutor("[ \"id1\", \"id2\" ]")
                .V()
                .Id()
                .ToArrayAsync());
        }

        [Fact]
        public async Task String_Ids2()
        {
            await Verify(await _g
                .WithExecutor("[ \"1\", \"2\" ]")
                .V()
                .Id()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Int_Ids()
        {
            await Verify(await _g
                .WithExecutor("[ 1, 2 ]")
                .V()
                .Id()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Empty_to_ints()
        {
            await Verify(await _g
                .WithExecutor("[{ \"Item1\": [], \"Item2\": [] }]")
                .V<(int[] ints, string[] strings)>()
                .ToArrayAsync());   //Must be Verify(...).
        }

        [Fact]
        public async Task Mixed_Ids()
        {
            await Verify(await _g
                .WithExecutor("[ 1, \"id2\" ]")
                .V()
                .Id()
                .ToArrayAsync());
        }

        [Fact]
        public async Task DateTime_is_UTC()
        {
            await Verify(await _g
                .WithExecutor(SingleCompanyJson)
                .V<Company>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task SingleCompany_dynamic()
        {
            await Verify(await _g
                .WithExecutor(SingleCompanyJson)
                .V<dynamic>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_unknown_type()
        {
            await Verify(await _g
                .WithExecutor(SingleLanguageJson)
                .V<object>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_unknown_type_without_model()
        {
            await Verify(await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .WithExecutor(SingleLanguageJson)
                .V()
                .Cast<object>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SingleLanguageJson)
                .V<Language>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_strongly_typed_without_model()
        {
            await Verify(await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .WithExecutor(SingleLanguageJson)
                .V()
                .Cast<Language>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_to_generic_vertex()
        {
            await Verify(await _g
                .WithExecutor(SingleLanguageJson)
                .V<Vertex>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Languages_to_object()
        {
            await Verify(await _g
                .WithExecutor(ArrayOfLanguages)
                .V<object>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Person_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SinglePersonJson)
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Person_with_null()
        {
            await Verify(await _g
                .WithExecutor(SinglePersonWithNullJson)
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Person_StringId()
        {
            await Verify(await _g
                .WithExecutor(SinglePersonStringId)
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Person_lowercase_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SinglePersonLowercasePropertiesJson)
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Person_without_PhoneNumbers_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SinglePersonWithoutPhoneNumbersJson)
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task TimeFrame_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SingleTimeFrameJson)
                .V<TimeFrame>()
                .ToArrayAsync());
        }

        [Fact(Skip = "Not standard behaviour!")]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            await Verify(await _g
                .WithExecutor(SingleTimeFrameWithNumbersJson)
                .V<TimeFrame>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            await Verify(await _g
                .WithExecutor(SingleLanguageJson)
                .V()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Tuple()
        {
            await Verify(await _g
                .WithExecutor(TupleOfPersonLanguageJson)
                .V()
                .Cast<(Person, Language)>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Tuple_vertex_vertex()
        {
            await Verify(await _g
                .WithExecutor(TupleOfPersonLanguageJson)
                .V()
                .Cast<(Vertex, Vertex)>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task NamedTuple()
        {
            await Verify(await _g
                .WithExecutor(NamedTupleOfPersonLanguageJson)
                .V()
                .Cast<PersonLanguageTuple>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Graphson3_Tuple()
        {
            await Verify(await _g
                .WithExecutor(Graphson3TupleOfPersonLanguageJson)
                .V()
                .Cast<(Person, Language)>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task SingleVertex_as_array()
        {
            await Verify(await _g
                .WithExecutor("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]")
                .V<Person>()
                .Fold()
                .ToArrayAsync());
        }

        [Fact]
        public async Task SingleVertex_as_array_of_arrays()
        {
            await Verify(await _g
                .WithExecutor("[ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27\", \"label\": \"vertex\", \"type\": \"vertex\", \"properties\": { \"PartitionKey\": [ { \"id\": \"3110d0db-17c0-4f82-89d8-0a7e9ae41c27|PartitionKey\", \"value\": \"p\" } ] } } ]")
                .V<Person>()
                .Fold()
                .Fold()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Array()
        {
            await Verify(await _g
                .WithExecutor(ArrayOfLanguages)
                .V()
                .Cast<Language[]>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Nested_Array()
        {
            await Verify(await _g
                .WithExecutor(NestedArrayOfLanguagesJson)
                .V()
                .Cast<Language[][]>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Scalar()
        {
            await Verify(await _g
                .WithExecutor("[ 36 ]")
                .V()
                .Cast<int>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Meta_Properties()
        {
            await Verify(await _g
                .WithExecutor(CountryWithMetaProperties)
                .V<Country>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task VertexProperties()
        {
            await Verify(await _g
                .WithExecutor(GetJson("VertexProperties"))
                .V()
                .Properties()
                .ToArrayAsync());
        }

        [Fact]
        public async Task VertexProperties_with_model()
        {
            await Verify(await _g
                .WithExecutor(GetJson("VertexProperties"))
                .V()
                .Properties()
                .Meta<MetaPoco>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task MetaProperties()
        {
            await Verify(await _g
                .WithExecutor(GetJson("Properties"))
                .V()
                .Properties()
                .Properties()
                .ToArrayAsync());
        }

        [Fact]
        public async Task VertexPropertyWithoutProperties()
        {
            await Verify(await _g
                .WithExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]")
                .V<Person>()
                .Properties(x => x.SomeObscureProperty!)
                .ToArrayAsync());
        }

        [Fact]
        public async Task VertexPropertyWithDateTimeOffset()
        {
            await Verify(await _g
                .WithExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]")
                .V<Person>()
                .Properties(x => x.Name!)
                .ToArrayAsync());
        }

        [Fact]
        public async Task PropertyWithDateTimeOffset()
        {
            await Verify(await _g
                .WithExecutor("{ \"@type\": \"g:List\",\"@value\": [ { \"@type\": \"g:Property\", \"@value\": { \"key\": \"ValidFrom\", \"value\": { \"@type\": \"g:Date\", \"@value\": 1548169812555 } } } ] }")
                .V<Person>()
                .Properties(x => x.Name!)
                .Properties(x => x.ValidFrom)
                .ToArrayAsync());
        }

        [Fact]
        public async Task BulkSet()
        {
            await Verify(await _g
                .WithExecutor("{ \"@type\": \"g:BulkSet\", \"@value\": [ { \"@type\": \"g:Vertex\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 69 }, \"label\": \"Person\" } }, { \"@type\": \"g:Int64\", \"@value\": 1 } ] }")
                .V<Person>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Traverser()
        {
            await Verify(await _g
                .WithExecutor(ThreeCompaniesAsTraverser)
                .V<Company>()
                .ToArrayAsync());
        }

        [Fact]
        public async Task Nullable()
        {
            await Verify(await _g
                .WithExecutor("[ { \"Item1\": [],  \"Item2\": [], \"Item3\": \"someString\", \"Item4\": \"someString\", \"Item5\": [],  \"Item5\": null } ]")
                .V<(string, string?, string, string?, int?, int?)>()
                .ToArrayAsync());
        }

        private static string GetJson(string name)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"ExRam.Gremlinq.Core.Tests.Json.{name}.json")).ReadToEnd();
        }

        private Task Verify<TElement>(TElement element)
        {
            var settings = new VerifySettings();
            settings.UseExtension("json");

            return Verifier.Verify(element, settings);
        }
    }
}
