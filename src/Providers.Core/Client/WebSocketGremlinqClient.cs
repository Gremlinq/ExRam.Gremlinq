using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
        private record struct ResponseMessageEnvelope(Guid? RequestId);

        private ClientWebSocket? _client;

        private readonly Uri _uri;
        private readonly SemaphoreSlim _sendLock = new(1);
        private readonly SemaphoreSlim _receiveLock = new(1);
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, Action<ReadOnlyMemory<byte>>> _finishActions = new();

        public WebSocketGremlinqClient(Uri uri, IGremlinQueryEnvironment environment)
        {
            _uri = uri;
            _environment = environment;
        }

        public async Task<ResponseMessage<T>> SendAsync<T>(RequestMessage message, CancellationToken ct)
        {
            var client = _client;
            var tcs = new TaskCompletionSource<ResponseMessage<T>>();

            while (true)
            {
                await _sendLock.WaitAsync(ct);

                try
                {
                    if (client is not null)
                    {
                        if (_environment.Serializer.TryTransform(message, _environment, out byte[]? serializedRequest))
                        {
                            if (!AddCallback(message.RequestId, tcs))
                                throw new InvalidOperationException();

                            await client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
                        }
                    }
                    else
                    {
                        client = new ClientWebSocket();
                        client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");

                        if (Interlocked.CompareExchange(ref _client, client, null) == null)
                            await client.ConnectAsync(_uri, ct);
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
                    using (var bytes = await client.ReceiveAsync(ct))
                    {
                        if (_environment.Deserializer.TryTransform(bytes.Memory, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                        {
                            if (responseMessageEnvelope.RequestId is { } requestId && _finishActions.TryRemove(requestId, out var finishAction))
                                finishAction(bytes.Memory);
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

