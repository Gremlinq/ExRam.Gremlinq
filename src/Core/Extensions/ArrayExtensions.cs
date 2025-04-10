using System.Collections.Immutable;

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
    }
}
