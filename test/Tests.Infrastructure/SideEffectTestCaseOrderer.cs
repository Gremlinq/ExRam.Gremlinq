using Xunit.Sdk;
using Xunit.v3;

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

            public int Compare(TTestCase? x, TTestCase? y) => GetIndex(x!.TestMethod!.MethodName).CompareTo(GetIndex(y!.TestMethod!.MethodName));

            private static int GetIndex(string str) => str.StartsWith("Drop")
                ? 0
                : str.StartsWith("Add")
                    ? 1
                    : 2;
        }

        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases) where TTestCase : notnull, ITestCase
        {
            return testCases
                .Where(testCase =>
                {
                    if (testCase.Traits.TryGetValue("Category", out var categories) && categories.Contains("IntegrationTest"))
                        return testCase.Traits.TryGetValue("ValidPlatform", out var validPlatforms) && validPlatforms.Any(validPlatform => OperatingSystem.IsOSPlatform(validPlatform));

                    return true;
                })
                .OrderBy(x => x, TestCaseComparer<TTestCase>.Instance)
                .ThenBy(x => x!.TestMethod!.MethodName, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
    }
}
