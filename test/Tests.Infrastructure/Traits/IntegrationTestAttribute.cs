using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    [TraitDiscoverer("ExRam.Gremlinq.Tests.Infrastructure.IntegrationTestDiscoverer", "ExRam.Gremlinq.Tests.Infrastructure")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
    {
        public IntegrationTestAttribute(string validPlatform, bool canRunOnCI = false)
        {
            CanRunOnCI = canRunOnCI;
            ValidPlatform = validPlatform;
        }

        public bool CanRunOnCI { get; }
        public string ValidPlatform { get; }
    }
}
