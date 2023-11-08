using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
        private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

        private record struct ResponseStatus(ResponseStatusCode Code);

        private ClientWebSocket? _client;

        private readonly GremlinServer _server;
        private readonly SemaphoreSlim _sendLock = new(1);
        private readonly SemaphoreSlim _receiveLock = new(1);
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, Action<ReadOnlyMemory<byte>>> _finishActions = new();

        public WebSocketGremlinqClient(GremlinServer server, IGremlinQueryEnvironment environment)
        {
            _server = server;
            _environment = environment;
        }

        public async Task<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message, CancellationToken ct)
        {
            var client = _client;
            var tcs = new TaskCompletionSource<ResponseMessage<T>>();

            if (!AddCallback(message.RequestId, tcs))
                throw new InvalidOperationException();

            while (true)
            {
                await _sendLock.WaitAsync(ct);

                try
                {
                    if (client is not null)
                    {
                        if (_environment.Serializer.TryTransform(message, _environment, out byte[]? serializedRequest))
                        {
                            await client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
                        }
                    }
                    else
                    {
                        client = new ClientWebSocket();
                        client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");

                        if (Interlocked.CompareExchange(ref _client, client, null) == null)
                            await client.ConnectAsync(_server.Uri, ct);
                        else
                            client = null;

                        continue;
                    }
                }
                finally
                {
                    _sendLock.Release();
                }

                await _receiveLock.WaitAsync(ct);

                try
                {
                    while (true)
                    {
                        using (var bytes = await client.ReceiveAsync(ct))
                        {
                            if (_environment.Deserializer.TryTransform(bytes.Memory, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                            {
                                if (responseMessageEnvelope.Status is { Code: Authenticate })
                                {
                                    var authMessage = RequestMessage
                                        .Build(Tokens.OpsAuthentication)
                                        .Processor(Tokens.ProcessorTraversal)
                                        .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{_server.Username}\0{_server.Password}")))
                                        .Create();

                                    if (_environment.Serializer.TryTransform(authMessage, _environment, out byte[]? serializedRequest))
                                    {
                                        await _sendLock.WaitAsync(ct);

                                        try
                                        {
                                            await client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
                                        }
                                        finally
                                        {
                                            _sendLock.Release();
                                        }
                                    }
                                }
                                else
                                {
                                    if (responseMessageEnvelope.RequestId is { } requestId && _finishActions.TryRemove(requestId, out var finishAction))
                                        finishAction(bytes.Memory);

                                    break;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    _receiveLock.Release();
                }

                return await tcs.Task;
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private bool AddCallback<T>(Guid requestId, TaskCompletionSource<ResponseMessage<T>> tcs) => _finishActions.TryAdd(
            requestId,
            bytes =>
            {
                try
                {
                    if (_environment.Deserializer.TryTransform(bytes, _environment, out ResponseMessage<T>? response))
                        tcs.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
    }
}

