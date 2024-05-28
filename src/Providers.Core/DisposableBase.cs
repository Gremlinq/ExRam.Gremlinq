namespace ExRam.Gremlinq.Providers.Core
{
    internal abstract class DisposableBase : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                DisposeImpl();
                GC.SuppressFinalize(this);
            }
        }

        protected abstract void DisposeImpl();
    }
}
