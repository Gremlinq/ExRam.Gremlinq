using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class SideEffectTestCaseOrderer : ITestCaseOrderer
    {
        private sealed class TestCaseComparer<TTestCase> : IComparer<TTestCase>
             where TTestCase : ITestCase
        {
            public static readonly TestCaseComparer<TTestCase> Instance = new();

            private TestCaseComparer()
            {
            }

            public int Compare(TTestCase? x, TTestCase? y) => GetIndex(x!.TestMethod.Method.Name).CompareTo(GetIndex(y!.TestMethod.Method.Name));

            private static int GetIndex(string str) => str.StartsWith("Drop")
                ? 0
                : str.StartsWith("Add")
                    ? 1
                    : 2;
        }

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases
                .Where(testCase =>
                {
                    if (testCase.Traits.TryGetValue("Category", out var categories) && categories.Contains("IntegrationTest"))
                    {
                        if (testCase.DisplayName.Split('.') is [.., { } providerName, _, _, _])
                            return Environment.Version.Major == 8 && Environment.GetEnvironmentVariable($"Run{providerName}IntegrationTests") is { } env && bool.TryParse(env, out var enabled) && enabled;
                    }

                    return true;
                })
                .OrderBy(x => x, TestCaseComparer<TTestCase>.Instance)
                .ThenBy(x => x!.TestMethod.Method.Name, StringComparer.OrdinalIgnoreCase);
        }

    }
}
