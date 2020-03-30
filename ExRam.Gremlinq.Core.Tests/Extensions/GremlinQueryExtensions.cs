using System.Threading.Tasks;
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
        }

        public static void VerifyQuery(this IGremlinQueryBase query, VerifyBase verifyBase)
        {
            var environment = query.AsAdmin().Environment;
            var serializedQuery = environment.Serializer
                .Serialize(query);

            Task.Run(() => verifyBase.Verify(serializedQuery, Settings)).Wait();
        }
    }
}
