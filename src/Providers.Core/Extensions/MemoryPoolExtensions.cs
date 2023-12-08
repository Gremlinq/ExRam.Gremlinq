using CommunityToolkit.HighPerformance.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class MemoryPoolExtensions
    {
        public static MemoryOwner<T> Double<T>(this MemoryOwner<T> owner)
        {
            using (owner)
            {
                var newOwner = MemoryOwner<T>.Allocate(owner.Memory.Length * 2);

                owner.Memory.CopyTo(newOwner.Memory);

                return newOwner;
            }
        }
    }
}
