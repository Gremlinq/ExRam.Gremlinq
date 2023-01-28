using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.Tests
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

        public static IGremlinQueryEnvironment EchoGraphsonString(this IGremlinQueryEnvironment environment)
        {
            return environment
                .UseSerializer(GremlinQuerySerializer.Default)
                .UseExecutor(GremlinQueryExecutor.Identity)
                .ConfigureDeserializer(static _ => _
                    .ToGraphsonString());
        }

        public static IGremlinQueryEnvironment EchoGroovyGremlinQuery(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureSerializer(static serializer => serializer.ToGroovy())
                .UseExecutor(GremlinQueryExecutor.Identity)
                .UseDeserializer(GremlinQueryFragmentDeserializer.Default);
        }
    }
}
