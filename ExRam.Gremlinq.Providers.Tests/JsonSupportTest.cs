using System;
using System.Collections.Generic;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Newtonsoft.Json.Linq;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Tests
{
    public class JsonSupportTest
    {
        private sealed class TestJsonQueryExecutor : IGremlinQueryExecutor
        {
            private readonly string _json;

            public TestJsonQueryExecutor(string json)
            {
                _json = json;
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable
                    .Return(JToken.Parse(_json))
                    .GraphsonDeserialize<TElement[]>(new GraphsonDeserializer(query.AsAdmin().Model))
                    .SelectMany(x => x.ToAsyncEnumerable());
            }
        }

        private static readonly string SingleUserJson;
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleCompanyJson;
        private static readonly string SingleUserStringId;
        private static readonly string SingleLanguageJson;
        private static readonly string SingleIsDescribedIn;
        private static readonly string SingleTimeFrameJson;
        private static readonly string TupleOfUserLanguageJson;
        private static readonly string Graphson3ReferenceVertex;
        private static readonly string CountryWithMetaProperties;
        private static readonly string NestedArrayOfLanguagesJson;
        private static readonly string SingleTimeFrameWithNumbersJson;
        private static readonly string SingleUserWithoutPhoneNumbersJson;
        private static readonly string SingleUserLowercasePropertiesJson;
        private static readonly string Graphson3TupleOfUserLanguageJson;

        private readonly IConfigurableGremlinQuerySource _g;

        static JsonSupportTest()
        {
            SingleLanguageJson = GetJson("Single_Language");
            SingleCompanyJson = GetJson("Single_Company");
            SingleUserJson = GetJson("Single_User");
            SingleUserLowercasePropertiesJson = GetJson("Single_User_lowercase_properties");
            SingleUserWithoutPhoneNumbersJson = GetJson("Single_User_without_PhoneNumbers");
            TupleOfUserLanguageJson = GetJson("Tuple_of_User_Language");
            ArrayOfLanguages = GetJson("Array_of_Languages");
            NestedArrayOfLanguagesJson = GetJson("Nested_array_of_Languages");
            SingleTimeFrameJson = GetJson("Single_TimeFrame");
            SingleTimeFrameWithNumbersJson = GetJson("Single_TimeFrame_with_numbers");
            SingleIsDescribedIn = GetJson("Single_IsDescribedIn");
            Graphson3TupleOfUserLanguageJson = GetJson("Graphson3_Tuple_of_User_Language");
            Graphson3ReferenceVertex = GetJson("Graphson3ReferenceVertex");
            CountryWithMetaProperties = GetJson("Country_with_meta_properties");
            SingleUserStringId = GetJson("Single_User_String_Id");
        }

        public JsonSupportTest()
        {
            _g = g
                .WithModel(GraphModel.FromBaseTypes<Vertex, Edge>(x => x.Id, x => x.Id));
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor(Graphson3ReferenceVertex))
                .V()
                .Cast<JObject>()
                .ToArray();

            array.Should().HaveCount(1);
            array[0]["id"].ToObject<int>().Should().Be(1);
            array[0]["label"].ToObject<string>().Should().Be("person");
            array[0]["name"].ToObject<string[]>(new GraphsonDeserializer(GraphModel.Empty)).Should().BeEquivalentTo("marko");
            array[0]["location"].ToObject<string[]>(new GraphsonDeserializer(GraphModel.Empty)).Should().BeEquivalentTo("san diego", "santa cruz", "brussels", "santa fe");
        }

        [Fact]
        public async Task IsDescribedIn()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleIsDescribedIn))
                .V<IsDescribedIn>()
                .ToArray();

            array.Should().HaveCount(1);
            array[0].Text.Should().Be("Deutsch");
        }

        [Fact]
        public async Task IsDescribedIn_with_Graphson3()
        {
            var array = await _g
                .WithExecutor(new TestJsonQueryExecutor("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"IsDescribedIn\",\"inVLabel\":\"Language\",\"outVLabel\":\"Country\",\"inV\":\"x-language:de\",\"outV\":\"ea46d1643c6d4dce9d7ac23fb09fb4b2\",\"properties\":{\"Text\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Text\",\"value\":\"Deutschland\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}"))
                .V<IsDescribedIn>()
                .ToArray();

            array.Should().HaveCount(1);
            array[0].Text.Should().Be("Deutschland");
        }

        [Fact]
        public async Task Empty1()
        {
            await _g
                .WithExecutor(new TestJsonQueryExecutor("[]"))
                .V()
                .Drop()
                .FirstOrDefault();
        }

        [Fact]
        public async Task Empty2()
        {
            await _g
                .WithExecutor(new TestJsonQueryExecutor("[]"))
                .V<User>()
                .ToArray();
        }

        [Fact]
        public async Task String_Ids()
        {
            var ids = await _g
                .WithExecutor(new TestJsonQueryExecutor("[ \"id1\", \"id2\" ]"))
                .V()
                .Id()
                .ToArray();

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
                .ToArray();

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
                .ToArray();

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
                .ToArray();

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
                .First();

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
                .First();

            language.Should().NotBeNull();
            language.Should().BeOfType<Language>();
            language.As<Language>().Id.Should().Be(10);
            language.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_unknown_type_without_model()
        {
            var language = await _g
                .WithModel(GraphModel.Empty)
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V()
                .Cast<object>()
                .First();

            language.Should().NotBeNull();
            language.Should().BeOfType<JObject>();
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            var language = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V<Language>()
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be(10);
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_strongly_typed_without_model()
        {
            var language = await _g
                .WithModel(GraphModel.Empty)
                .WithExecutor(new TestJsonQueryExecutor(SingleLanguageJson))
                .V()
                .Cast<Language>()
                .First();

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
                .First();

            vertex.Should().BeOfType<Language>();
            vertex.Should().NotBeNull();
            vertex.Id.Should().Be(10);
        }
        
        [Fact]
        public async Task User_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleUserJson))
                .V<User>()
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be(13);
            user.Age.Should().Be(36);
            user.Gender.Should().Be(Gender.Female);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_StringId()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleUserStringId))
                .V<User>()
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("13");
            user.Age.Should().Be(36);
            user.Gender.Should().Be(Gender.Female);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_lowercase_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleUserLowercasePropertiesJson))
                .V<User>()
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be(14);
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_without_PhoneNumbers_strongly_typed()
        {
            var user = await _g
                .WithExecutor(new TestJsonQueryExecutor(SingleUserWithoutPhoneNumbersJson))
                .V<User>()
                .First();

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
                .First();

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
                .First();

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
                .V().First() as Language;

            language.Should().NotBeNull();
            language?.Id.Should().Be(10);
            language?.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(TupleOfUserLanguageJson))
                .V()
                .Cast<(User, Language)>()
                .First();

            tuple.Item1.Id.Should().Be(16);
            tuple.Item1.Name.Should().Be("Name of some base entity");
            tuple.Item1.Age.Should().Be(36);

            tuple.Item2.Id.Should().Be(17);
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple_vertex_vertex()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(TupleOfUserLanguageJson))
                .V()
                .Cast<(Vertex, Vertex)>()
                .First();

            tuple.Item1.Id.Should().Be(16);
            tuple.Item1.Should().BeOfType<User>();
            tuple.Item1.As<User>().Name.Should().Be("Name of some base entity");
            tuple.Item1.As<User>().Age.Should().Be(36);

            tuple.Item2.Id.Should().Be(17);
            tuple.Item2.Should().BeOfType<Language>();
            tuple.Item2.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Graphson3_Tuple()
        {
            var tuple = await _g
                .WithExecutor(new TestJsonQueryExecutor(Graphson3TupleOfUserLanguageJson))
                .V()
                .Cast<(User, Language)>()
                .First();

            tuple.Item1.Id.Should().Be(4);
            tuple.Item1.Name.Should().Be("Name of some base entity");
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
                .First();

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
                .First();

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
                .First();

            value.Should().Be(36);
        }

        [Fact]
        public async Task Meta_Properties()
        {
            var country = await _g
                .WithExecutor(new TestJsonQueryExecutor(CountryWithMetaProperties))
                .V<Country>()
                .First();

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
                .ToArray(default);

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
        public async Task MetaProperties()
        {
            var properties = await _g
                .WithExecutor(new TestJsonQueryExecutor(GetJson("Properties")))
                .V()
                .Properties()
                .Properties()
                .ToArray();

            properties.Should().HaveCount(2);
            properties[0].Key.Should().Be("metaKey1");
            properties[0].Value.Should().Be("metaValue1");
            properties[1].Key.Should().Be("metaKey2");
            properties[1].Value.Should().Be(36);
        }

        private static string GetJson(string name)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"ExRam.Gremlinq.Providers.Tests.Json.{name}.json")).ReadToEnd();
        }
    }
}
