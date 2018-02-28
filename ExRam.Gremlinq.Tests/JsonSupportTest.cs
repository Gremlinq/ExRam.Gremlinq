using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class JsonSupportTest
    {
        private static readonly string SingleUserJson;
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleLanguageJson;
        private static readonly string SingleTimeFrameJson;
        private static readonly string TupleOfUserLanguageJson;
        private static readonly string NestedArrayOfLanguagesJson;
        private static readonly string SingleTimeFrameWithNumbersJson;
        private static readonly string SingleUserWithoutPhoneNumbersJson;
        private static readonly string SingleUserLowercasePropertiesJson;

        static JsonSupportTest()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            SingleLanguageJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_Language.json")).ReadToEnd();
            SingleUserJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_User.json")).ReadToEnd();
            SingleUserLowercasePropertiesJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_User_lowercase_properties.json")).ReadToEnd();
            SingleUserWithoutPhoneNumbersJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_User_without_PhoneNumbers.json")).ReadToEnd();
            TupleOfUserLanguageJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Tuple_of_User_Language.json")).ReadToEnd();
            ArrayOfLanguages = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Array_of_Languages.json")).ReadToEnd();
            NestedArrayOfLanguagesJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Nested_array_of_Languages.json")).ReadToEnd();
            SingleTimeFrameJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_TimeFrame.json")).ReadToEnd();
            SingleTimeFrameWithNumbersJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_TimeFrame_with_numbers.json")).ReadToEnd();
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Fact]
        public async Task Empty1()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[]"));

            await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").V().Drop())
                .FirstOrDefault();
        }

        [Fact]
        public async Task Empty2()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[]"));

            await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").V<User>())
                .ToArray();
        }

        [Fact]
        public async Task Language_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleLanguageJson));

            var language = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<Language>())
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task User_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserJson));

            var user = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2017, 12, 1, 15, 28, 24, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_lowercase_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserLowercasePropertiesJson));

            var user = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().Equal("+123456", "+234567");
            user.RegistrationDate.Should().Be(new DateTimeOffset(2017, 12, 1, 15, 28, 24, TimeSpan.Zero));
        }

        [Fact]
        public async Task User_without_PhoneNumbers_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleUserWithoutPhoneNumbersJson));

            var user = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<User>())
                .First();

            user.Should().NotBeNull();
            user.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            user.Age.Should().Be(36);
            user.PhoneNumbers.Should().BeEmpty();
            user.RegistrationDate.Should().Be(new DateTimeOffset(2017, 12, 1, 15, 28, 24, TimeSpan.Zero));
        }

        [Fact]
        public async Task TimeFrame_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleTimeFrameJson));

            var timeFrame = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<TimeFrame>())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task TimeFrame_with_numbers_strongly_typed()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleTimeFrameWithNumbersJson));

            var timeFrame = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<TimeFrame>())
                .First();

            timeFrame.Should().NotBeNull();
            timeFrame.Id.Should().Be("15da4cea93114bfc8c6b23847487d97b");
            timeFrame.StartTime.Should().Be(new TimeSpan(6, 0, 0));
            timeFrame.Duration.Should().Be(new TimeSpan(16, 0, 0));
        }

        [Fact]
        public async Task Language_by_vertex_inheritance()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(SingleLanguageJson));

            var language = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<Vertex>())
                .First() as Language;

            language.Should().NotBeNull();
            language?.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language?.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public async Task Tuple()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(TupleOfUserLanguageJson));

            var jsonQueryProvider = queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport();              

            var tuple = await GremlinQuery
                .Create("g")
                .Cast<(User, Language)>()
                .Execute(jsonQueryProvider)
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(ArrayOfLanguages));

            var languages = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<Language[]>())
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
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return(NestedArrayOfLanguagesJson));

            var languages = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create("g").Cast<Language[][]>())
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
    }
}
