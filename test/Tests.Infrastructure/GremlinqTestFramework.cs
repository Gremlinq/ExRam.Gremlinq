using System.Globalization;
using System.Reflection;

using Xunit.Sdk;
using Xunit.v3;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public sealed class GremlinqTestFramework : XunitTestFramework
    {
        private sealed class GremlinqTestFrameworkExecutor : ITestFrameworkExecutor
        {
            private readonly ITestFrameworkExecutor _baseExecutor;

            public GremlinqTestFrameworkExecutor(ITestFrameworkExecutor baseExecutor)
            {
                _baseExecutor = baseExecutor;
            }

            public ValueTask RunTestCases(IReadOnlyCollection<ITestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions, CancellationToken? cancellationToken = null) => _baseExecutor
                .RunTestCases(
                    testCases
                        .Where(testCase =>
                        {
#if Gremlinq_Extensions
                            if (testCase.TestClassNamespace?.StartsWith("Gremlinq.Extensions") is false)
                                return false;
#elif ExRam_Gremlinq
                            if (testCase.TestClassNamespace?.StartsWith("ExRam.Gremlinq") is false)
                                return false;
#endif

                            if (testCase.Traits.TryGetValue("Category", out var categories) && categories.Contains("IntegrationTest"))
                                return testCase.Traits.TryGetValue("ValidPlatform", out var validPlatforms) && validPlatforms.Any(validPlatform => OperatingSystem.IsOSPlatform(validPlatform));

                            return true;
                        })
                        .ToArray(),
                    executionMessageSink,
                    executionOptions);
        }

        static GremlinqTestFramework()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        }

        public override string TestFrameworkDisplayName => nameof(GremlinqTestFramework);

        protected override ITestFrameworkDiscoverer CreateDiscoverer(Assembly assembly) => base.CreateDiscoverer(assembly);

        protected override ITestFrameworkExecutor CreateExecutor(Assembly assembly) => new GremlinqTestFrameworkExecutor(base.CreateExecutor(assembly));
    }
}
