using System;
using System.Linq;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryTest
    {
        [Fact]
        public void ChangeQueryType()
        {
            var anon = GremlinQuerySource.g.V().AsAdmin();

            var interfaces = typeof(GremlinQuery)
                .Assembly
                .GetTypes()
                .Where(iface => iface.IsInterface)
                .Where(iface => typeof(IGremlinQueryBase).IsAssignableFrom(iface))
                .Where(iface => !iface.Name.Contains("Rec"))
                .Select(iface => iface.IsGenericTypeDefinition
                    ? iface.MakeGenericType(iface.GetGenericArguments().Select(_ => typeof(object)).ToArray())
                    : iface)
                .ToArray();

            foreach (var iface in interfaces)
            {
                typeof(IGremlinQueryAdmin).GetMethod(nameof(IGremlinQueryAdmin.ChangeQueryType)).MakeGenericMethod(iface).Invoke(anon, Array.Empty<object>());
            }
        }

        [Fact]
        public void ChangeQueryType_optimizes()
        {
            var query = GremlinQuerySource.g.V<Person>();

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQuery<Person>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase>()
                .Should()
                .NotBeSameAs(query);

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);
        }
    }
}
