using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.WebSocket;

using Gremlin.Net.Process.Traversal;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutorExtensions
    {
        private sealed class LoggingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, Action<object, Guid>> Loggers = new();

            private readonly IGremlinQueryExecutor _executor;

            public LoggingGremlinQueryExecutor(IGremlinQueryExecutor executor)
            {
                _executor = executor;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var requestId = Guid.NewGuid();

                    await using (var enumerator = _executor.Execute(serializedQuery, environment).GetAsyncEnumerator(ct))
                    {
                        try
                        {
                            var moveNext = enumerator.MoveNextAsync();

                            var logger = Loggers.GetValue(
                                environment,
                                environment => GetLoggingFunction(environment));

                            logger(serializedQuery, requestId);

                            if (!await moveNext)
                                yield break;
                        }
                        catch (Exception ex)
                        {
                            environment.Logger.LogError($"Error executing Gremlin query with id { requestId }: {ex}");

                            throw;
                        }

                        do
                        {
                            yield return enumerator.Current;
                        } while (await enumerator.MoveNextAsync());
                    }
                }
            }

            private static Action<object, Guid> GetLoggingFunction(IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogLogLevel);
                var verbosity = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogVerbosity);
                var formatting = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogFormatting);
                var groovyFormatting = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogGroovyFormatting);

                return (serializedQuery, requestId) =>
                {
                    if (environment.Logger.IsEnabled(logLevel))
                    {
                        var gremlinQuery = serializedQuery switch
                        {
                            Bytecode bytecode => bytecode.ToGroovy(groovyFormatting),
                            GroovyGremlinQuery groovyGremlinQuery => groovyFormatting == GroovyFormatting.Inline
                                ? groovyGremlinQuery.Inline()
                                : groovyGremlinQuery,
                            _ => throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.")
                        };

                        environment.Logger.Log(
                            logLevel,
                            "Executing Gremlin query {0}.",
                            JsonConvert.SerializeObject(
                                new
                                {
                                    RequestId = requestId,
                                    gremlinQuery.Script,
                                    Bindings = (verbosity & QueryLogVerbosity.IncludeBindings) > QueryLogVerbosity.QueryOnly
                                        ? gremlinQuery.Bindings
                                        : null
                                },
                                formatting));
                    }
                };
            }
        }

        public static IGremlinQueryExecutor Log(this IGremlinQueryExecutor executor) => new LoggingGremlinQueryExecutor(executor);
    }
}
