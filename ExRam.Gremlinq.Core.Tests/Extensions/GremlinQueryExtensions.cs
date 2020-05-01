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
        }

        public static async Task Verify(this IGremlinQueryBase query, VerifyBase verifyBase)
        {
            await verifyBase.Verify(
                JsonConvert.SerializeObject(
                    await query
                        .Cast<object>()
                        .ToArrayAsync(),
                    Formatting.Indented),
                Settings);
        }
    }
}
