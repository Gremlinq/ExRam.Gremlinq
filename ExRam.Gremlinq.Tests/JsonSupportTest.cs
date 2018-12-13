using System;
using FluentAssertions;
using Moq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class JsonSupportTest
    {
        private static readonly string SingleUserJson;
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleCompanyJson;
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
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(Graphson3ReferenceVertex)));

            var array = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.Empty)
                .Execute(g
                    .V<JObject>())
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
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleIsDescribedIn)));

            var array = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<IsDescribedIn>())
                .ToArray();

            array.Should().HaveCount(1);
            array[0].Text.Should().Be("Deutsch");
        }

        [Fact]
        public async Task IsDescribedIn_with_Graphson3()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"IsDescribedIn\",\"inVLabel\":\"Language\",\"outVLabel\":\"Country\",\"inV\":\"x-language:de\",\"outV\":\"ea46d1643c6d4dce9d7ac23fb09fb4b2\",\"properties\":{\"Text\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Text\",\"value\":\"Deutschland\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}")));

            var array = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V<IsDescribedIn>())
                .ToArray();

            array.Should().HaveCount(1);
            array[0].Text.Should().Be("Deutschland");
        }

        [Fact]
        public async Task Empty1()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[]")));

            await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V()
                    .Drop())
                .FirstOrDefault();
        }

        [Fact]
        public async Task Empty2()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[]")));

            await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V<User>())
                .ToArray();
        }

        [Fact]
        public async Task String_Ids()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[ \"id1\", \"id2\" ]")));

            var ids = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V()
                    .Id())
                .ToArray();

            ids.Should().HaveCount(2);
            ids[0].Should().Be("id1");
            ids[1].Should().Be("id2");
        }

        [Fact]
        public async Task String_Ids2()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[ \"1\", \"2\" ]")));

            var ids = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V()
                    .Id())
                .ToArray();

            ids.Should().HaveCount(2);
            ids[0].Should().Be("1");
            ids[1].Should().Be("2");
        }

        [Fact]
        public async Task Int_Ids()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[ 1, 2 ]")));

            var ids = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V().Id())
                .ToArray();

            ids.Should().HaveCount(2);
            ids[0].Should().Be(1);
            ids[1].Should().Be(2);
        }

        [Fact]
        public async Task Mixed_Ids()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[ 1, \"id2\" ]")));

            var ids = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V()
                    .Id())
                .ToArray();

            ids.Should().HaveCount(2);
            ids[0].Should().Be(1);
            ids[1].Should().Be("id2");
        }

        [Fact]
        public async Task DateTime_is_UTC()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleCompanyJson)));

            var user = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<Company>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.FoundingDate.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public async Task Language_unknown_type()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleLanguageJson)));

            var language = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<object>())
                .First();

            language.Should().NotBeNull();
            language.Should().BeOfType<Language>();
            language.As<Language>().Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleLanguageJson)));

            var language = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<Language>())
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Language_to_generic_vertex()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleLanguageJson)));

            var language = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.Empty)// .FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<Vertex>())
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.Label.Should().Be("Language");
        }


        [Fact]
        public async Task User_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleUserJson)));

            var user = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.Gender.Should().Be(Gender.Female);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_lowercase_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleUserLowercasePropertiesJson)));

            var user = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_without_PhoneNumbers_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleUserWithoutPhoneNumbersJson)));

            var user = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().BeEmpty();
            user.RegistrationDate.Should().Be(new DateTimeOffset(2016, 12, 14, 21, 14, 36, 295, TimeSpan.Zero));
        }

        [Fact]
        public async Task TimeFrame_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleTimeFrameJson)));

            var timeFrame = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<TimeFrame>())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleTimeFrameWithNumbersJson)));

            var timeFrame = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<TimeFrame>())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(SingleLanguageJson)));

            var language = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V())
                .First() as Language;

            language.Should().NotBeNull();
            language?.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language?.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(TupleOfUserLanguageJson)));

            var jsonQueryProvider = queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));              

            var tuple = await g
                .WithQueryProvider(jsonQueryProvider)
                .V()
                .Cast<(User, Language)>()
                .Execute()
                .First();

            tuple.Item1.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            tuple.Item1.Name.Should().Be("Name of some base entity");
            tuple.Item1.Age.Should().Be(36);

            tuple.Item2.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple_vertex_vertex()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(TupleOfUserLanguageJson)));

            var jsonQueryProvider = queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));

            var tuple = await g
                .WithQueryProvider(jsonQueryProvider)
                .V()
                .Cast<(Vertex, Vertex)>()
                .Execute()
                .First();

            tuple.Item1.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            tuple.Item1.Should().BeOfType<User>();
            tuple.Item1.As<User>().Name.Should().Be("Name of some base entity");
            tuple.Item1.As<User>().Age.Should().Be(36);

            tuple.Item2.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            tuple.Item2.Should().BeOfType<Language>();
            tuple.Item2.As<Language>().IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Graphson3_Tuple()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(Graphson3TupleOfUserLanguageJson)));

            var jsonQueryProvider = queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));

            var tuple = await g
                .WithQueryProvider(jsonQueryProvider)
                .V()
                .Cast<(User, Language)>()
                .Execute()
                .First();

            tuple.Item1.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            tuple.Item1.Name.Should().Be("Name of some base entity");
            tuple.Item1.Age.Should().Be(36);

            tuple.Item2.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Array()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(ArrayOfLanguages)));

            var languages = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<Language[]>())
                .First();

            languages.Should().NotBeNull();
            languages.Should().HaveCount(2);
            languages[0].Id.Should().Be("f788f4dcfe2b48b4a5abf40af17892d7");
            languages[0].IetfLanguageTag.Should().Be("de");
            languages[1].Id.Should().Be("9556f62ca30147568702924c648d494c");
            languages[1].IetfLanguageTag.Should().Be("en");
        }

        [Fact]
        public async Task Nested_Array()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(NestedArrayOfLanguagesJson)));

            var languages = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<Language[][]>())
                .First();

            languages.Should().NotBeNull();
            languages.Should().HaveCount(2);
            languages[0].Should().NotBeNull();
            languages[0].Should().HaveCount(1);
            languages[0][0].Id.Should().Be("4da59ae1600f4b60a4e319b70661d8f2");
            languages[0][0].IetfLanguageTag.Should().Be("en");
            languages[1].Should().NotBeNull();
            languages[1].Should().HaveCount(2);
            languages[1][0].Id.Should().Be("bd3530fc586e43a1b43e8fdb2771c8f8");
            languages[1][0].IetfLanguageTag.Should().Be("de");
            languages[1][1].Id.Should().Be("4da59ae1600f4b60a4e319b70661d8f2");
            languages[1][1].IetfLanguageTag.Should().Be("en");
        }

        [Fact]
        public async Task Scalar()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse("[ 36 ]")));

            var value = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V<int>())
                .First();

            value.Should().Be(36);
        }

        [Fact]
        public async Task Meta_Properties()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(CountryWithMetaProperties)));

            var c = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge),
                    GraphElementNamingStrategy.Simple))
                .Execute(g.V<Country>())
                .First();

            c.Name.Value.Should().Be("GER");
            c.Name.Properties["de"].Should().Be("Deutschland");
            c.Name.Properties["en"].Should().Be("Germany");
        }

        [Fact]
        public async Task VertexProperties()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(GetJson("VertexProperties"))));

            var properties = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.Empty)
                .Execute(g.V().Properties())
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
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<JToken>>()))
                .Returns(AsyncEnumerable.Return(JToken.Parse(GetJson("Properties"))));

            var properties = await queryProviderMock.Object
                .WithJsonSupport(GraphModel.Empty)
                .Execute(g.V().Properties().Properties())
                .ToArray(default);

            properties.Should().HaveCount(2);
            properties[0].Key.Should().Be("metaKey1");
            properties[0].Value.Should().Be("metaValue1");
            properties[1].Key.Should().Be("metaKey2");
            properties[1].Value.Should().Be(36);
        }

        private static string GetJson(string name)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"ExRam.Gremlinq.Tests.Json.{name}.json")).ReadToEnd();
        }
    }
}
