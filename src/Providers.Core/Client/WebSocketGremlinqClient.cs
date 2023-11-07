using System.Buffers;
using System.Collections.Concurrent;
using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class MemoryPoolExtensions
    {
        public static IMemoryOwner<T> Double<T>(this MemoryPool<T> pool, IMemoryOwner<T> memory)
        {
            using (memory)
            {
                var newMemory = pool.Rent(memory.Memory.Length * 2);

                memory.Memory.CopyTo(newMemory.Memory);

                return newMemory;
            }
        }
    }

    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
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
                    var read = 0;
                    var bytes = MemoryPool<byte>.Shared.Rent(2048);

                    try
                    {
                        var result = await client.ReceiveAsync(bytes.Memory, ct);

                        read = result.Count;

                        while (!result.EndOfMessage)
                        {
                            if (read == bytes.Memory.Length)
                                bytes = MemoryPool<byte>.Shared.Double(bytes);

                            result = await client.ReceiveAsync(bytes.Memory[read..], ct);
                            read += result.Count;
                        }

                        var segment = (ReadOnlyMemory<byte>)bytes.Memory[..read];

                        if (_environment.Deserializer.TryTransform(segment, _environment, out ResponseMessage<List<object>>? responseMessage))
                        {
                            if (responseMessage.RequestId is { } requestId && _finishActions.TryRemove(requestId, out var finishAction))
                                finishAction(segment);
                        }
                    }
                    finally
                    {
                        bytes.Dispose();
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
    }
}
