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
            Settings.DisableDiff();
        }

        public static Task Verify(this IGremlinQueryBase query, VerifyBase verifyBase)
        {
            return verifyBase.Verify(
                query
                    .AsAdmin()
                    .Environment
                    .Serializer
                    .Serialize(query),
                Settings);
        }
    }
}
