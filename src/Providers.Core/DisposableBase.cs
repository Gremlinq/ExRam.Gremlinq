namespace ExRam.Gremlinq.Providers.Core
{
    internal abstract class DisposableBase : IDisposable
    {
        private int _disposed;

        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                Dispose();
                GC.SuppressFinalize(this);
            }
        }

        protected abstract void Dispose();
    }
}
