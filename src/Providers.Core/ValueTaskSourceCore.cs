using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;

namespace ExRam.Gremlinq.Providers.Core
{
    [StructLayout(LayoutKind.Auto)]
    internal struct ValueTaskSourceCore<TResult>
    {
        private int _isSet;
        private ManualResetValueTaskSourceCore<TResult> _valueTaskSource;

        public bool TrySetResult(TResult result)
        {
            var wasSet = Interlocked.CompareExchange(ref _isSet, 1, 0) == 0;

            if (wasSet)
                _valueTaskSource.SetResult(result);

            return wasSet;
        }

        public ValueTaskSourceStatus GetStatus(short token) => token == 0
            ? _valueTaskSource.GetStatus(0)
            : throw new InvalidOperationException();

        public TResult GetResult(short token) => token == 0
            ? _valueTaskSource.GetResult(0)
            : throw new InvalidOperationException();

        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            if (token != 0)
                throw new InvalidOperationException();

            _valueTaskSource.OnCompleted(continuation, state, token, flags);
        }
    }
}
