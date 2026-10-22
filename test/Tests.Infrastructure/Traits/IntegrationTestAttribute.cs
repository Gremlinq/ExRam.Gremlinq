using Xunit.v3;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
    {
#if LATEST_TARGET_FRAMEWORK
        private static readonly bool IsLatestTargetFramework = true;
#else
        private static readonly bool IsLatestTargetFramework = false;
#endif

        public IntegrationTestAttribute(string validPlatform, bool canRunOnCI = false)
        {
            CanRunOnCI = canRunOnCI;
            ValidPlatform = validPlatform;
        }

        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            return [.. Core()];

            IEnumerable<KeyValuePair<string, string>> Core()
            {
                var isCi = bool.TryParse(Environment.GetEnvironmentVariable("CI"), out var ci) && ci;

                yield return new KeyValuePair<string, string>("Category", "IntegrationTest");

                if (!isCi || CanRunOnCI && IsLatestTargetFramework)
                    yield return new KeyValuePair<string, string>("ValidPlatform", ValidPlatform);
            }
        }

        public bool CanRunOnCI { get; }
        public string ValidPlatform { get; }
    }
}
