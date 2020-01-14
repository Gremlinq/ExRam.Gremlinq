using System;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    public struct GremlinQueryAwaiter<TElement> : ICriticalNotifyCompletion
    {
        private readonly TaskAwaiter<TElement[]> _valueTaskAwaiter;

        internal GremlinQueryAwaiter(TaskAwaiter<TElement[]> valueTaskAwaiter)
        {
            _valueTaskAwaiter = valueTaskAwaiter;
        }

        public TElement[] GetResult()
        {
            return _valueTaskAwaiter.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            _valueTaskAwaiter.OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _valueTaskAwaiter.UnsafeOnCompleted(continuation);
        }

        public bool IsCompleted { get => _valueTaskAwaiter.IsCompleted; }
    }
}
