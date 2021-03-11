using System.Linq;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class QuerySemanticsTest : VerifyBase
    {
        public QuerySemanticsTest() : base()
        {

        }

        [Fact]
        public Task Mappings()
        {
            return Verify(typeof(IGremlinQueryBase)
                .Assembly
                .GetTypes()
                .Where(x => x.IsInterface)
                .ToDictionary(x => x, x => x.TryGetQuerySemantics()));
        }
    }
}
