using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class XunitLogger : ILogger, IDisposable
        {
            private readonly ITestOutputHelper _output;

            public XunitLogger(ITestOutputHelper output)
            {
                _output = output;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string>? formatter)
            {
                _output.WriteLine(state?.ToString());
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                return this;
            }

            public void Dispose()
            {
            }
        }

        public static IGremlinQueryEnvironment LogToXunit(this IGremlinQueryEnvironment environment, ITestOutputHelper testOutputHelper)
        {
            return environment
                .UseLogger(new XunitLogger(testOutputHelper));
        }
    }
}
