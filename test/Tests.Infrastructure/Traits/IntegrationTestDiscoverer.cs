using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class IntegrationTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var isCi = bool.TryParse(Environment.GetEnvironmentVariable("CI"), out var ci)
                ? ci
                : false;

            yield return new KeyValuePair<string, string>("Category", "IntegrationTest");

            if (traitAttribute is ReflectionAttributeInfo { Attribute: IntegrationTestAttribute integrationTestAttribute })
            {
                if (/*integrationTestAttribute.CanRunOnCI || */ !isCi)
                    yield return new KeyValuePair<string, string>("ValidPlatform", integrationTestAttribute.ValidPlatform);
            }
        }
    }
}
