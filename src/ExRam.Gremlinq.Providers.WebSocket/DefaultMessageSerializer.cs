using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    internal sealed class DefaultMessageSerializer : IMessageSerializer
    {
        private readonly IGremlinQueryEnvironment _environment;

        public DefaultMessageSerializer(IGremlinQueryEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage, CancellationToken ct) => _environment.Serializer
            .TransformTo<byte[]>()
            .From(requestMessage, _environment);

        public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message, CancellationToken ct) => message.Length == 0
            ? null!
            : _environment.Deserializer.TryTransformTo<ResponseMessage<List<object>>>().From(message, _environment) is { } responseMessage
                ? responseMessage
                : throw new InvalidOperationException("""
                       No deserializer configured!
                       In Gremlinq v12, query result deserialization has been decoupled from the core library.
                       To keep using Newtonsoft.Json as Json-deserialization mechanism, add a reference to
                       ExRam.Gremlinq.Support.NewtonsoftJson (or ExRam.Gremlinq.Support.NewtonsoftJson.AspNet on ASP.NET Core)
                       and call 'UseNewtonsoftJson()' in the provider configuration.

                       Examples:

                       Provider configuration

                           IGremlinQuerySource g = ...

                           g = g.UseCosmosDb(c => c
                                   .UseNewtonsoftJson());

                       ASP.NET Core

                           IServiceCollection services = ...

                           services.AddGremlinq(setup => setup
                               .UseCosmosDb(providerSetup => providerSetup
                                   .UseNewtonsoftJson()));

                       Manual configuration

                           IGremlinQuerySource g = ...

                           g = g.ConfigureEnvironment(env => env
                               .UseNewtonsoftJson());
                       
                       """);
    }
}
