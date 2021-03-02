using System.Linq;
using System.Threading.Tasks;
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
                var data = JsonConvert.SerializeObject(
                    await query
                        .ToArrayAsync(),
                    Formatting.Indented);

                var serialized = testBase
                    .Scrubbers()
                    .Aggregate(data, (s, func) => func(s));

                await testBase.Verify(serialized);
            }
        }
    }
}
