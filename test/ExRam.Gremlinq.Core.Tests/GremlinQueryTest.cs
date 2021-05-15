using System;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryTest : GremlinqTestBase
    {
        private readonly IGremlinQuerySource _g;

        public GremlinQueryTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _g = g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public void ChangeQueryType()
        {
            var anon = _g
                .V()
                .AsAdmin();

            var interfaces = typeof(IGremlinQueryBase)
                .Assembly
                .GetTypes()
                .Where(iface => iface.IsInterface)
                .Where(iface => typeof(IGremlinQueryBase).IsAssignableFrom(iface))
                .Where(iface => !iface.Name.Contains("Rec"))
                .Select(iface => iface.IsGenericTypeDefinition
                    ? iface.MakeGenericType(iface.GetGenericArguments().Select(_ => typeof(Person)).ToArray())
                    : iface)
                .ToArray();

            foreach (var iface in interfaces)
            {
                typeof(IGremlinQueryAdmin).GetMethod(nameof(IGremlinQueryAdmin.ChangeQueryType))!.MakeGenericMethod(iface).Invoke(anon, Array.Empty<object>());
            }
        }

        [Fact]
        public void ChangeQueryType_optimizes()
        {
            var query = _g
                .V<Person>();

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);

            query.AsAdmin().ChangeQueryType<IEdgeGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);
        }

        [Fact]
        public void ChangeQueryType_takes_array_element_types_into_account()
        {
            var query = _g
                .V<Person>();

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase<Person[]>>()
                .Should()
                .BeAssignableTo<IArrayGremlinQueryBase<Person>>();
        }

        [Fact]
        public async Task Debug()
        {
            await Verify(_g
                .V<Person>()
                .Where(x => x.Age > 36)
                .Out<LivesIn>()
                .OfType<Country>()
                .Debug());
        }

        [Fact]
        public async Task ForceVertex_has_correct_semantics()
        {
            var s = _g
                .V<Person>()
                .Count()
                .ForceVertex()
                .AsAdmin().Projection;

            await Verify(s);
        }
    }
}
