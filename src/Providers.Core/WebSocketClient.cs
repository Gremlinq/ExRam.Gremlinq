using System.Collections.Concurrent;
using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinqClient : IDisposable
    {
        Task<ResponseMessage<T>> SendAsync<T>(RequestMessage message, CancellationToken ct);
    }

    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
        private ClientWebSocket? _client;

        private readonly Uri _uri;
        private readonly SemaphoreSlim _sendLock = new (1);
        private readonly SemaphoreSlim _receiveLock = new (1);
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, Action<byte[]>> _finishActions = new();

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
                            _finishActions.TryAdd(
                                message.RequestId,
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

                            await client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
                        }
                    }
                    else
                    {
                        client = new ClientWebSocket();
                        client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");

                        if (Interlocked.CompareExchange(ref _client, client, null) == null)
                            await client.ConnectAsync(_uri, ct).ConfigureAwait(false);
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
                    var read = 0;
                    var bytes = new byte[16 * 1024];
                    var result = await client.ReceiveAsync(bytes.AsMemory(), ct);

                    read = result.Count;

                    while (!result.EndOfMessage)
                    {
                        var newBytes = new byte[bytes.Length * 2];
                        bytes.AsSpan()[..result.Count].CopyTo(newBytes);
                        bytes = newBytes;

                        result = await client.ReceiveAsync(bytes.AsMemory()[result.Count..], ct);

                        read += result.Count;
                    }

                    bytes = bytes.AsSpan().Slice(0, read).ToArray();

                    if (_environment.Deserializer.TryTransform(bytes, _environment, out ResponseMessage<List<object>>? responseMessage))
                    {
                        if (responseMessage.RequestId is { } requestId && _finishActions.TryRemove(requestId, out var finishAction))
                            finishAction(bytes);
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
            throw new NotImplementedException();
        }
    }
}
