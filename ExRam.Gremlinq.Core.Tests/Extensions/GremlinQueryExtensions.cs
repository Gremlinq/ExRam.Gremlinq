using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Verify;
using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        private static readonly VerifySettings Settings = new VerifySettings();

        static GremlinQueryExtensions()
        {
            Settings.UseExtension("json");
            Settings.DisableDiff();

#if (DEBUG)
            Settings.AutoVerify();
#endif
        }

        public static async Task Verify<TElement>(this IGremlinQueryBase<TElement> query, VerifyBase verifyBase)
        {
            if (verifyBase is QuerySerializationTest && typeof(TElement) != typeof(object))
            {
                await query.Cast<object>().Verify(verifyBase);
            }
            else if (verifyBase is QueryIntegrationTest && typeof(TElement) != typeof(JToken))
            {
                await query.Cast<JToken>().Verify(verifyBase);
            }
            else
            {
                var data = await query
                    .ToArrayAsync();

                await verifyBase.Verify(
                    JsonConvert.SerializeObject(
                        data,
                        Formatting.Indented),
                    Settings);
            }
        }
    }
}
