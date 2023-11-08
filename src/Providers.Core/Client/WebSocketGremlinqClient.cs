using System.Collections;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
        private sealed class State<T>
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly List<ResponseMessage<T>> _list = new ();
            private readonly TaskCompletionSource<ResponseMessage<T>[]> _tcs = new ();

            public State(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public void Signal(ReadOnlyMemory<byte> bytes)
            {
                try
                {
                    if (_environment.Deserializer.TryTransform(bytes, _environment, out ResponseMessage<T>? response))
                    {
                        _list.Add(response);

                        if (response.Status.Code != PartialContent)
                            _tcs.TrySetResult(_list.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    _tcs.TrySetException(ex);
                }
            }

            public Task<ResponseMessage<T>[]> Responses => _tcs.Task;
        }

        private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

        private record struct ResponseStatus(ResponseStatusCode Code);

        private readonly GremlinServer _server;
        private readonly ClientWebSocket _client = new();
        private readonly SemaphoreSlim _sendLock = new(1);
        private readonly SemaphoreSlim _receiveLock = new(1);
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, Action<ReadOnlyMemory<byte>>> _finishActions = new();

        public WebSocketGremlinqClient(GremlinServer server, IGremlinQueryEnvironment environment)
        {
            _server = server;
            _environment = environment;

            _client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");
        }

        public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
        {
            return Core(message);

            async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, [EnumeratorCancellation] CancellationToken ct = default)
            {
                var state = new State<T>(_environment);

                _finishActions.TryAdd(message.RequestId, state.Signal);

                var loopTask = Loop(message, ct);

                try
                {
                    foreach(var response in await state.Responses)
                    {
                        yield return response;
                    }

                    await loopTask;
                }
                finally
                {
                    _finishActions.TryRemove(message.RequestId, out _);
                }
            }

            async Task Loop(RequestMessage message, CancellationToken ct)
            {
                await SendCore(message, ct);

                while (true)
                {
                    var (envelope, bytes) = await ReceiveCore(ct);

                    using (bytes)
                    {
                        if (envelope.Status is { Code: Authenticate })
                        {
                            var authMessage = RequestMessage
                                .Build(Tokens.OpsAuthentication)
                                .Processor(Tokens.ProcessorTraversal)
                                .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{_server.Username}\0{_server.Password}")))
                                .Create();

                            await SendCore(authMessage, ct);
                        }
                        else
                        {
                            if (envelope.RequestId is { } requestId && _finishActions.TryGetValue(requestId, out var finishAction))
                                finishAction(bytes.Memory);

                            if (envelope.Status?.Code is not PartialContent)
                                break;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private async Task SendCore(RequestMessage requestMessage, CancellationToken ct)
        {
            await _sendLock.WaitAsync(ct);

            try
            {
                if (_client.State == WebSocketState.None)
                    await _client.ConnectAsync(_server.Uri, ct);

                if (_environment.Serializer.TryTransform(requestMessage, _environment, out byte[]? serializedRequest))
                    await _client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task<(ResponseMessageEnvelope Envelope, ClientWebSocketExtensions.SlicedMemoryOwner Bytes)> ReceiveCore(CancellationToken ct)
        {
            await _receiveLock.WaitAsync(ct);

            try
            {
                var bytes = await _client.ReceiveAsync(ct);

                if (_environment.Deserializer.TryTransform(bytes.Memory, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                    return (responseMessageEnvelope, bytes);

                using (bytes)
                {
                    throw new InvalidOperationException();
                }
            }
            finally
            {
                _receiveLock.Release();
            }
        }
    }
}

