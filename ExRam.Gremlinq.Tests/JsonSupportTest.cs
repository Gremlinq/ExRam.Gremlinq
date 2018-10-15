using System;
using System.Collections.Generic;
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
        }

        [Fact]
        public async Task GraphSon3ReferenceVertex()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(Graphson3ReferenceVertex));

            var array = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.Empty)
                .Execute(g
                    .V<JObject>())
                .ToArray();

            array.Should().HaveCount(1);
            array[0]["id"].ToObject<int>().Should().Be(1);
            array[0]["label"].ToObject<string>().Should().Be("person");
            array[0]["name"].ToObject<string[]>().Should().BeEquivalentTo("marko");
            array[0]["location"].ToObject<string[]>().Should().BeEquivalentTo("san diego", "santa cruz", "brussels", "santa fe");
        }

        [Fact]
        public async Task IsDescribedIn()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleIsDescribedIn));

            var array = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g.V<IsDescribedIn>())
                .ToArray();

            array.Should().HaveCount(1);
            array[0].Text.Should().Be("Deutsch");
        }

        [Fact]
        public async Task IsDescribedIn_with_Graphson3()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("{\"@type\":\"g:List\",\"@value\":[{\"@type\":\"g:Edge\",\"@value\":{\"id\":{\"@type\":\"g:Int64\",\"@value\":23},\"label\":\"IsDescribedIn\",\"inVLabel\":\"Language\",\"outVLabel\":\"Country\",\"inV\":\"x-language:de\",\"outV\":\"ea46d1643c6d4dce9d7ac23fb09fb4b2\",\"properties\":{\"Text\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"Text\",\"value\":\"Deutschland\"}},\"ActiveFrom\":{\"@type\":\"g:Property\",\"@value\":{\"key\":\"ActiveFrom\",\"value\":{\"@type\":\"g:Int64\",\"@value\":1523879885819}}}}}}]}"));

            var array = await queryProviderMock.Object
                .Select(JToken.Parse)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[]"));

            await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V()
                    .Drop())
                .FirstOrDefault();
        }

        [Fact]
        public async Task Empty2()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[]"));

            await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(g
                    .V<User>())
                .ToArray();
        }

        [Fact]
        public async Task String_Ids()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ \"id1\", \"id2\" ]"));

            var ids = await queryProviderMock.Object
                .Select(JToken.Parse)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ \"1\", \"2\" ]"));

            var ids = await queryProviderMock.Object
                .Select(JToken.Parse)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ 1, 2 ]"));

            var ids = await queryProviderMock.Object
                .Select(JToken.Parse)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ 1, \"id2\" ]"));

            var ids = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery
                    .Create("g")
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleCompanyJson));

            var user = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<Company>
                    .Create())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.FoundingDate.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleLanguageJson));

            var language = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<Language>
                    .Create())
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task User_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserJson));

            var user = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<User>
                    .Create())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserLowercasePropertiesJson));

            var user = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<User>
                    .Create())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserWithoutPhoneNumbersJson));

            var user = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<User>
                    .Create())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleTimeFrameJson));

            var timeFrame = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<TimeFrame>
                    .Create())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleTimeFrameWithNumbersJson));

            var timeFrame = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<TimeFrame>
                    .Create())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleLanguageJson));

            var language = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<Vertex>
                    .Create())
                .First() as Language;

            language.Should().NotBeNull();
            language?.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language?.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(TupleOfUserLanguageJson));

            var jsonQueryProvider = queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));              

            var tuple = await GremlinQuery
                .Create("g")
                .SetTypedGremlinQueryProvider(jsonQueryProvider)
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
        public async Task Graphson3_Tuple()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(Graphson3TupleOfUserLanguageJson));

            var jsonQueryProvider = queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));

            var tuple = await GremlinQuery
                .Create("g")
                .SetTypedGremlinQueryProvider(jsonQueryProvider)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(ArrayOfLanguages));

            var languages = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<Language[]>
                    .Create())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(NestedArrayOfLanguagesJson));

            var languages = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery<Language[][]>.Create())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ 36 ]"));

            var value = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery
                    .Create("g")
                    .Cast<int>())
                .First();

            value.Should().Be(36);
        }

        [Fact]
        public async Task Count()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("36"));

            var value = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .Execute(GremlinQuery
                    .Create("g")
                    .Cast<int>())
                .First();

            value.Should().Be(36);
        }

        private static string GetJson(string name)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"ExRam.Gremlinq.Tests.Json.{name}.json")).ReadToEnd();
        }
    }
}
