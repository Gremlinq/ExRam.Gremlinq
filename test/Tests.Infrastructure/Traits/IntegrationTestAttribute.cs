using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    [TraitDiscoverer("ExRam.Gremlinq.Tests.Infrastructure.IntegrationTestDiscoverer", "ExRam.Gremlinq.Tests.Infrastructure")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
    {

    }
}
