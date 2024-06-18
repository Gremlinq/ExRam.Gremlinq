using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class IntegrationTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", "IntegrationTest");

            if (traitAttribute is ReflectionAttributeInfo { Attribute: IntegrationTestAttribute integrationTestAttribute })
            {
                foreach (var validPlatform in integrationTestAttribute.ValidPlatforms)
                {
                    yield return new KeyValuePair<string, string>("ValidPlatform", validPlatform);
                }
            }
        }
    }
}
