using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public static async Task Verify(this IGremlinQueryBase query, VerifyBase verifyBase)
        {
            var data = await query
                .Cast<object>()
                .ToArrayAsync();

            await verifyBase.Verify(
                JsonConvert.SerializeObject(
                    data,
                    Formatting.Indented),
                Settings);
        }
    }
}
