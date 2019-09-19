using System;
using System.Collections.Generic;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;
using LanguageExt;
using Newtonsoft.Json.Linq;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Tests
{
    public static class ConfigurableGremlinSourceExtensions
    {
        public static IConfigurableGremlinQuerySource WithExecutor(this IConfigurableGremlinQuerySource source, IGremlinQueryExecutor executor)
        {
            return source.ConfigureExecutionPipeline(pipeline => pipeline
                .UseGroovySerialization()
                .UseExecutor(executor)
                .UseGraphsonDeserialization());
        }
    }

    public class JsonSupportTest
    {
        private sealed class TestJsonQueryExecutor : IGremlinQueryExecutor
        {
            private readonly string _json;

            public TestJsonQueryExecutor(string json)
            {
                _json = json;
            }

            public IAsyncEnumerable<object> Execute(object groovySerializedQuery)
            {
                return AsyncEnumerableEx
                    .Return(JToken.Parse(_json));
            }
        }

        private sealed class MetaPoco
        {
            public string MetaKey { get; set; }
        }

        private static readonly string SinglePersonJson;
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleCompanyJson;
        private static readonly string SinglePersonStringId;
        private static readonly string SingleLanguageJson;
        private static readonly string SingleWorksFor;
        private static readonly string SingleTimeFrameJson;
        private static readonly string TupleOfPersonLanguageJson;
        private static readonly string Graphson3ReferenceVertex;
        private static readonly string CountryWithMetaProperties;
        private static readonly string NestedArrayOfLanguagesJson;
        private static readonly string SingleTimeFrameWithNumbersJson;
        private static readonly string SinglePersonWithoutPhoneNumbersJson;
        private static readonly string SinglePersonLowercasePropertiesJson;
        private static readonly string Graphson3TupleOfPersonLanguageJson;

        private readonly IConfigurableGremlinQuerySource _g;

        static JsonSupportTest()
        {
            SingleLanguageJson = GetJson("Single_Language");
            SingleCompanyJson = GetJson("Single_Company");
            SinglePersonJson = GetJson("Single_Person");
            SinglePersonLowercasePropertiesJson = GetJson("Single_Person_lowercase_properties");
            SinglePersonWithoutPhoneNumbersJson = GetJson("Single_Person_without_PhoneNumbers");
            TupleOfPersonLanguageJson = GetJson("Tuple_of_Person_Language");
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

        public JsonSupportTest()
        {
            _g = g
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>());
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor(Graphson3ReferenceVertex))
                .V()
                .Cast<JObject>()
                .ToArrayAsync();

            array.Should().HaveCount(1);
            array[0]["id"].ToObject<int>().Should().Be(1);
            array[0]["label"].ToObject<string>().Should().Be("person");
            array[0]["name"].ToObject<string[]>(new GraphsonJsonSerializer(GremlinQueryEnvironment.Default)).Should().BeEquivalentTo("marko");
            array[0]["location"].ToObject<string[]>(new GraphsonJsonSerializer(GremlinQueryEnvironment.Default)).Should().BeEquivalentTo("san diego", "santa cruz", "brussels", "santa fe");
        }

        [Fact]
        public async Task Configured_property_name()
        {
            var persons = await _g
                .ConfigureModel(model => model
                    .ConfigureProperties(prop => prop
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement"))))
                .WithExecutor(new TestJsonQueryExecutor("[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]"))
                .V<Person>()
                .ToArrayAsync();

            persons
                .Should()
                .HaveCount(1);

            persons[0]
                .Name
                .Should()
                .NotBeNull();

            persons[0].Name.Value
                .Should()
                .Be("nameValue");
        }

        [Fact]
        public async Task IsDescribedIn()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleWorksFor))
                .E<WorksFor>()
                .ToArrayAsync();

            array.Should().HaveCount(1);
            array[0].Role.Should().Be("Admin");
        }

        [Fact]
        public async Task WorksFor_with_Graphson3()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"WorksFor\",\"inVLabel\":\"Company\",\"outVLabel\":\"Person\",\"inV\":\"companyId\",\"outV\":\"personId\",\"properties\":{\"Role\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Role\",\"value\":\"Admin\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}"))
                .E<WorksFor>()
                .ToArrayAsync();

            array.Should().HaveCount(1);
            array[0].Role.Should().Be("Admin");
        }

        [Fact]
        public async Task Empty1()
        {
            await _g
                .WithExecutor(new TestJsonQueryExecutor("[]"))
                .V()
                .Drop()
                .FirstOrDefaultAsync();
        }

        [Fact]
        public async Task Empty2()
        {
            await _g
                .WithExecutor(new TestJsonQueryExecutor("[]"))
                .V<Person>()
                .ToArrayAsync();
        }

        [Fact]
        public async Task String_Ids()
        {
            var ids = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ \"id1\", \"id2\" ]"))
                .V()
                .Id()
                .ToArrayAsync();

            ids.Should().HaveCount(2);
            ids[0].Should().Be("id1");
            ids[1].Should().Be("id2");
        }

        [Fact]
        public async Task String_Ids2()
        {
            var ids = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ \"1\", \"2\" ]"))
                .V()
                .Id()
                .ToArrayAsync();

            ids.Should().HaveCount(2);
            ids[0].Should().Be("1");
            ids[1].Should().Be("2");
        }

        [Fact]
        public async Task Int_Ids()
        {
            var ids = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ 1, 2 ]"))
                .V()
                .Id()
                .ToArrayAsync();

            ids.Should().HaveCount(2);
            ids[0].Should().Be(1);
            ids[1].Should().Be(2);
        }

        [Fact]
        public async Task Mixed_Ids()
        {
            var ids = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ 1, \"id2\" ]"))
                .V()
                .Id()
                .ToArrayAsync();

            ids.Should().HaveCount(2);
            ids[0].Should().Be(1);
            ids[1].Should().Be("id2");
        }

        [Fact]
        public async Task DateTime_is_UTC()
        {
            var company = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleCompanyJson))
                .V<Company>()
                .FirstAsync();

            company.Should().NotBeNull();
            company.Id.Should().Be("b9b89d7f-9313-4eed-b354-2760ba7a3fbe");
            company.FoundingDate.Kind.Should().Be(DateTimeKind.Utc);
            company.FoundingDate.Should().Be(new DateTime(2018, 12, 17, 8, 0, 0, DateTimeKind.Utc));
        }

        [Fact]
        public async Task Language_unknown_type()
        {
            var language = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V<object>()
                .FirstAsync();

            language.Should().NotBeNull();
            language.Should().BeOfType<Language>();
            language.As<Language>().Id.Should().Be(10);
            language.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_unknown_type_without_model()
        {
            var language = await _g
                .UseModel(GraphModel.Empty)
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V()
                .Cast<object>()
                .FirstAsync();

            language.Should().NotBeNull();
            language.Should().BeOfType<JObject>();
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            var language = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V<Language>()
                .FirstAsync();

            language.Should().NotBeNull();
            language.Id.Should().Be(10);
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_strongly_typed_without_model()
        {
            var language = await _g
                .UseModel(GraphModel.Empty)
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V()
                .Cast<Language>()
                .FirstAsync();

            language.Should().NotBeNull();
            language.Id.Should().Be(10);
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_to_generic_vertex()
        {
            var vertex = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V<Vertex>()
                .FirstAsync();

            vertex.Should().BeOfType<Language>();
            vertex.Should().NotBeNull();
            vertex.Id.Should().Be(10);
        }

        [Fact]
        public async Task Languages_to_object()
        {
            var vertices = await _g
                .WithExecutor(new TestJsonQueryExecutor(ArrayOfLanguages))
                .V<object>()
                .ToArrayAsync();

            vertices.Should().HaveCount(1);
            vertices[0].Should().BeOfType<JArray>();
        }

        [Fact]
        public async Task Person_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SinglePersonJson))
                .V<Person>()
                .FirstAsync();

            user.Should().NotBeNull();
            user.Id.Should().Be(13);
            user.Age.Should().Be(36);
            user.Gender.Should().Be(Gender.Female);
            user.PhoneNumbers.Select(x => x.Value).Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task Person_StringId()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SinglePersonStringId))
                .V<Person>()
                .FirstAsync();

            user.Should().NotBeNull();
            user.Id.Should().Be("13");
            user.Age.Should().Be(36);
            user.Gender.Should().Be(Gender.Female);
            user.PhoneNumbers.Select(x => x.Value).Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task Person_lowercase_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SinglePersonLowercasePropertiesJson))
                .V<Person>()
                .FirstAsync();

            user.Should().NotBeNull();
            user.Id.Should().Be(14);
            user.Age.Should().Be(36);
            user.PhoneNumbers.Select(x => x.Value).Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task Person_without_PhoneNumbers_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SinglePersonWithoutPhoneNumbersJson))
                .V<Person>()
                .FirstAsync();

            user.Should().NotBeNull();
            user.Id.Should().Be(15);
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().BeEmpty();
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task TimeFrame_strongly_typed()
        {
            var timeFrame = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleTimeFrameJson))
                .V<TimeFrame>()
                .FirstAsync();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be(11);
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact(Skip = "Not standard behaviour!")]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            var timeFrame = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleTimeFrameWithNumbersJson))
                .V<TimeFrame>()
                .FirstAsync();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be(12);
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            var language = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V().FirstAsync() as Language;

            language.Should().NotBeNull();
            language?.Id.Should().Be(10);
            language?.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(TupleOfPersonLanguageJson))
                .V()
                .Cast<(Person, Language)>()
                .FirstAsync();

            tuple.Item1.Id.Should().Be(16);
            tuple.Item1.Name.Value.Should().Be("Name of some base entity");
            tuple.Item1.Age.Should().Be(36);

            tuple.Item2.Id.Should().Be(17);
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple_vertex_vertex()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(TupleOfPersonLanguageJson))
                .V()
                .Cast<(Vertex, Vertex)>()
                .FirstAsync();

            tuple.Item1.Id.Should().Be(16);
            tuple.Item1.Should().BeOfType<Person>();
            tuple.Item1.As<Person>().Name.Value.Should().Be("Name of some base entity");
            tuple.Item1.As<Person>().Age.Should().Be(36);

            tuple.Item2.Id.Should().Be(17);
            tuple.Item2.Should().BeOfType<Language>();
            tuple.Item2.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Graphson3_Tuple()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(Graphson3TupleOfPersonLanguageJson))
                .V()
                .Cast<(Person, Language)>()
                .FirstAsync();

            tuple.Item1.Id.Should().Be(4);
            tuple.Item1.Name.Value.Should().Be("Name of some base entity");
            tuple.Item1.Age.Should().Be(36);

            tuple.Item2.Id.Should().Be(5);
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Array()
        {
            var languages = await _g
                .WithExecutor(new TestJsonQueryExecutor(ArrayOfLanguages))
                .V()
                .Cast<Language[]>()
                .FirstAsync();

            languages.Should().NotBeNull();
            languages.Should().HaveCount(2);
            languages[0].Id.Should().Be(1);
            languages[0].IetfLanguageTag.Should().Be("de");
            languages[1].Id.Should().Be(2);
            languages[1].IetfLanguageTag.Should().Be("en");
        }

        [Fact]
        public async Task Nested_Array()
        {
            var languages = await _g
                .WithExecutor(new TestJsonQueryExecutor(NestedArrayOfLanguagesJson))
                .V()
                .Cast<Language[][]>()
                .FirstAsync();

            languages.Should().NotBeNull();
            languages.Should().HaveCount(2);
            languages[0].Should().NotBeNull();
            languages[0].Should().HaveCount(1);
            languages[0][0].Id.Should().Be(6);
            languages[0][0].IetfLanguageTag.Should().Be("en");
            languages[1].Should().NotBeNull();
            languages[1].Should().HaveCount(2);
            languages[1][0].Id.Should().Be(7);
            languages[1][0].IetfLanguageTag.Should().Be("de");
            languages[1][1].Id.Should().Be(8);
            languages[1][1].IetfLanguageTag.Should().Be("en");
        }

        [Fact]
        public async Task Scalar()
        {
            var value = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ 36 ]"))
                .V()
                .Cast<int>()
                .FirstAsync();

            value.Should().Be(36);
        }

        [Fact]
        public async Task Meta_Properties()
        {
            var country = await _g
                .WithExecutor(new TestJsonQueryExecutor(CountryWithMetaProperties))
                .V<Country>()
                .FirstAsync();

            country.Name.Value.Should().Be("GER");
            country.Name.Properties["de"].Should().Be("Deutschland");
            country.Name.Properties["en"].Should().Be("Germany");
        }

        [Fact]
        public async Task VertexProperties()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor(GetJson("VertexProperties")))
                .V()
                .Properties()
                .ToArrayAsync();

            properties.Should().HaveCount(3);
            properties[0].Label.Should().Be("Property1");
            properties[0].Value.Should().Be(1540202009475);
            properties[1].Label.Should().Be("Property2");
            properties[1].Value.Should().Be("Some string");
            properties[2].Label.Should().Be("Property3");
            properties[2].Value.Should().Be(36);

            properties[0].Properties.Should().Contain("metaKey", "MetaValue");
        }

        [Fact]
        public async Task VertexProperties_with_model()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor(GetJson("VertexProperties")))
                .V()
                .Properties()
                .Meta<MetaPoco>()
                .ToArrayAsync();

            properties.Should().HaveCount(3);
            properties[0].Label.Should().Be("Property1");
            properties[0].Value.Should().Be(1540202009475);
            properties[1].Label.Should().Be("Property2");
            properties[1].Value.Should().Be("Some string");
            properties[2].Label.Should().Be("Property3");
            properties[2].Value.Should().Be(36);

            properties[0].Properties.MetaKey.Should().Be("MetaValue");
            properties[1].Properties.Should().BeNull();
            properties[2].Properties.Should().BeNull();
        }

        [Fact]
        public async Task MetaProperties()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor(GetJson("Properties")))
                .V()
                .Properties()
                .Properties()
                .ToArrayAsync();

            properties.Should().HaveCount(2);
            properties[0].Key.Should().Be("metaKey1");
            properties[0].Value.Should().Be("metaValue1");
            properties[1].Key.Should().Be("metaKey2");
            properties[1].Value.Should().Be(36);
        }

        [Fact]
        public async Task VertexPropertyWithoutProperties()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]"))
                .V<Person>()
                .Properties(x => x.SomeObscureProperty)
                .ToArrayAsync();

            properties.Should().HaveCount(1);
            properties[0].Properties.Should().NotBeNull();
        }

        [Fact]
        public async Task VertexPropertyWithDateTimeOffset()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]"))
                .V<Person>()
                .Properties(x => x.Name)
                .ToArrayAsync();

            properties.Should().HaveCount(1);
            properties[0].Properties.Should().NotBeNull();
            properties[0].Properties.ValidFrom.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(1548112365431));
        }

        [Fact]
        public async Task PropertyWithDateTimeOffset()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor("{ \"@type\": \"g:List\",\"@value\": [ { \"@type\": \"g:Property\", \"@value\": { \"key\": \"ValidFrom\", \"value\": { \"@type\": \"g:Date\", \"@value\": 1548169812555 } } } ] }"))
                .V<Person>()
                .Properties(x => x.Name)
                .Properties(x => x.ValidFrom)
                .ToArrayAsync();

            properties.Should().HaveCount(1);
            properties[0].Should().NotBeNull();
            properties[0].Value.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(1548169812555));
        }

        [Fact]
        public async Task Maybe()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ { \"Item1\": [],  \"Item2\": [], \"Item3\": \"someString\", \"Item4\": \"someString\", \"Item5\": [] } ]"))
                .V<(string, Option<string>, string, Option<string>, int?)>()
                .FirstAsync();

            tuple.Item1.Should().BeNull();
            tuple.Item2.IsSome.Should().BeFalse();
            tuple.Item3.Should().Be("someString");
            tuple.Item4.Should().BeEqual("someString");
            tuple.Item5.Should().BeNull();
        }

        private static string GetJson(string name)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"ExRam.Gremlinq.Core.Tests.Json.{name}.json")).ReadToEnd();
        }
    }
}
