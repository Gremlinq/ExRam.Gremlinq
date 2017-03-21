using FluentAssertions;
using Moq;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryProviderTest
    {
        private static readonly string TupleJson;
        private static readonly string LanguageJson1;

        static GremlinQueryProviderTest()
        {
            LanguageJson1 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Language1.json")).ReadToEnd();
            TupleJson = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExRam.Gremlinq.Tests.Json.Tuple.json")).ReadToEnd();
        }

        [Fact]
        public void Language_strongly_typed()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<string>>()))
                .Returns(new[] { LanguageJson1 });

            var language = queryProviderMock.Object
                .WithModel(GremlinModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge)))
                .WithNamingStrategy(GraphElementNamingStrategy.Simple)
                .WithJsonSupport()
                .Execute(Mock.Of<IGremlinQuery<Language>>(x => x.MemberInfoMappings == ImmutableDictionary<MemberInfo, string>.Empty))
                .First();

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public void Language_by_vertex_inheritance()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<string>>()))
                .Returns(new[] { LanguageJson1 });

            var language = queryProviderMock.Object
                .WithModel(GremlinModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge)))
                .WithNamingStrategy(GraphElementNamingStrategy.Simple)
                .WithJsonSupport()
                .Execute(Mock.Of<IGremlinQuery<Vertex>>(x => x.MemberInfoMappings == ImmutableDictionary<MemberInfo, string>.Empty))
                .First() as Language;

            language.Should().NotBeNull();
            language.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            language.IetfLanguageTag.Should().Be("de");
        }

        [Fact]
        public void Tuple()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<IGremlinQuery<string>>()))
                .Returns(new[] { TupleJson });

            var jsonQueryProvider = queryProviderMock.Object
                .WithModel(GremlinModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge)))
                .WithNamingStrategy(GraphElementNamingStrategy.Simple)
                .WithJsonSupport();              

            var tuple = GremlinQuery
                .ForGraph("g", jsonQueryProvider)
                .Cast<(SomeBaseEntity, Language)>()
                .AddMemberInfoMapping(x => x.Item1, "d730b14d9898459ab919d529939f69f8")
                .AddMemberInfoMapping(x => x.Item2, "38233d3440304ce7bae0be402687aced")
                .Execute()
                .First();

            tuple.Item1.Id.Should().Be("d13ef3f51c86496eb2c22823601446ad");
            tuple.Item1.Name.Should().Be("Name of some base entity");
            tuple.Item1.SomeIntProperty.Should().Be(36);

            tuple.Item2.Id.Should().Be("be66544bcdaa4ee9990eaf006585153b");
            tuple.Item2.IetfLanguageTag.Should().Be("de");
        }
    }
}
