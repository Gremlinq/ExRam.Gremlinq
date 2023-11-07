using System.Buffers;

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
}
