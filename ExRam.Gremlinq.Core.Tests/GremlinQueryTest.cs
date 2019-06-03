using System;
using System.Linq;
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
                .Where(iface => typeof(IGremlinQuery).IsAssignableFrom(iface))
                .Select(iface => iface.IsGenericTypeDefinition
                    ? iface.MakeGenericType(iface.GetGenericArguments().Select(_ => typeof(object)).ToArray())
                    : iface)
                .ToArray();

            foreach (var iface in interfaces)
            {
                typeof(IGremlinQueryAdmin).GetMethod(nameof(IGremlinQueryAdmin.ChangeQueryType)).MakeGenericMethod(iface).Invoke(anon, Array.Empty<object>());
            }
        }
    }
}
