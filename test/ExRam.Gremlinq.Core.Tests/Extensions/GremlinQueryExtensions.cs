using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        private static readonly VerifySettings Settings = new();
        private static readonly Regex IdRegex = new Regex("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:Int64\"\\s*,\\s*\"@value\":\\s*)([^\\s*{}]+)(\\s*})");

        static GremlinQueryExtensions()
        {
            Settings.UseExtension("json");
            Settings.DisableDiff();

#if (DEBUG)
            Settings.AutoVerify();
#endif
        }

        public static async Task Verify<TElement>(this IGremlinQueryBase<TElement> query, XunitContextBase contextBase)
        {
            if (contextBase is QuerySerializationTest && typeof(TElement) != typeof(object))
            {
                await query.Cast<object>().Verify(contextBase);
            }
            else if (contextBase is QueryIntegrationTest && typeof(TElement) != typeof(JToken))
            {
                await query.Cast<JToken>().Verify(contextBase);
            }
            else
            {
                var data = JsonConvert.SerializeObject(
                    await query
                        .ToArrayAsync(),
                    Formatting.Indented);

                var serialized = contextBase is QueryIntegrationTest
                    ? IdRegex.Replace(data, "$1\"scrubbed id\"$3")
                    : data;

                await Verifier.Verify(
                    serialized,
                    Settings,
                    contextBase.SourceFile);
            }
        }
    }
}
