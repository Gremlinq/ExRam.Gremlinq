using System.Linq;
using System.Threading.Tasks;

using ExRam.Gremlinq.Core.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public static async Task Verify<TElement>(this IGremlinQueryBase<TElement> query)
        {
            var testBase = GremlinqTestBase.Current;

            if (testBase is QuerySerializationTest && typeof(TElement) != typeof(object))
            {
                await query.Cast<object>().Verify();
            }
            else if (testBase is QueryIntegrationTest && typeof(TElement) != typeof(JToken))
            {
                await query.Cast<JToken>().Verify();
            }
            else
            {
                var serialized = JsonConvert.SerializeObject(
                    await query
                        .ToAsyncEnumerable()
                        .Select(x => x is BytecodeGremlinQuery query
                            ? query.Bytecode
                            : (object)x!)
                        .ToArrayAsync(),
                    Formatting.Indented);

                var scrubbed = testBase
                    .Scrubbers()
                    .Aggregate(serialized, (s, func) => func(s));

                await testBase.Verify(scrubbed);
            }
        }
    }
}
