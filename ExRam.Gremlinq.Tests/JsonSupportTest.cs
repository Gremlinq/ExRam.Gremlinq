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
        private static readonly string ArrayOfLanguages;
        private static readonly string SingleLanguageJson;
        private static readonly string SingleTimeFrameJson;
        private static readonly string TupleOfUserLanguageJson;
        private static readonly string NestedArrayOfLanguagesJson;

        static JsonSupportTest()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            SingleLanguageJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_Language.json")).ReadToEnd();
            TupleOfUserLanguageJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Tuple_of_User_Language.json")).ReadToEnd();
            ArrayOfLanguages = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Array_of_Languages.json")).ReadToEnd();
            NestedArrayOfLanguagesJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Nested_array_of_Languages.json")).ReadToEnd();
            SingleTimeFrameJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Single_TimeFrame.json")).ReadToEnd();
            // ReSharper restore AssignNullToNotNullAttribute
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
                .Execute(Mock.Of<IGremlinQuery<Language>>(x => x.StepLabelMappings == ImmutableDictionary<string, StepLabel>.Empty))
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
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
                .Execute(Mock.Of<IGremlinQuery<TimeFrame>>(x => x.StepLabelMappings == ImmutableDictionary<string, StepLabel>.Empty))
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
                .Execute(Mock.Of<IGremlinQuery<Vertex>>(x => x.StepLabelMappings == ImmutableDictionary<string, StepLabel>.Empty))
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
                .Execute(Mock.Of<IGremlinQuery<Language[]>>(x => x.StepLabelMappings == ImmutableDictionary<string, StepLabel>.Empty))
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
                .Execute(Mock.Of<IGremlinQuery<Language[][]>>(x => x.StepLabelMappings == ImmutableDictionary<string, StepLabel>.Empty))
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
