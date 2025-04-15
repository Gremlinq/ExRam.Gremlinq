using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ExRam.Gremlinq.Core
{
    internal static class ArrayExtensions
    {
        public static ImmutableArray<T> UnsafeToImmutableArray<T>(this T[] array) =>
#if NET8_0_OR_GREATER
            System.Runtime.InteropServices.ImmutableCollectionsMarshal.AsImmutableArray(array);
#else
            array.ToImmutableArray();
#endif

        public static ImmutableArray<T> UnsafeToImmutableArray<T>(this Memory<T> memory)
        {
            if (MemoryMarshal.TryGetArray<T>(memory, out var segment) && segment.Array is { } array)
            {
                if (segment.Count == array.Length)
                {
                    return array
                        .UnsafeToImmutableArray();
                }
            }

            return memory.Span
                .ToImmutableArray();
        }
    }
}
