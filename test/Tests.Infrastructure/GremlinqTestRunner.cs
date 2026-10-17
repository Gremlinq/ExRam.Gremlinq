using Xunit.v3;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    // xUnit awaits only Task, ValueTask and F# Async; it ignores every other result of a test method, so a fact that
    // returns a SettingsTask would pass without verifying. This runner converts the SettingsTask and refuses anything
    // else it does not know.
    public sealed class GremlinqTestRunner : XunitTestRunner
    {
        public static object? ConvertTestMethodResult(object? result) => result switch
        {
            SettingsTask settingsTask => settingsTask.ToTask(),
            null or Task or ValueTask => result,
            _ => throw new InvalidOperationException($"Test method returned a {result.GetType()}, which the test runner cannot await.")
        };

        protected override object? InvokeTestMethod(XunitTestRunnerContext ctxt, object? testClassInstance) => ConvertTestMethodResult(base.InvokeTestMethod(ctxt, testClassInstance));
    }
}
