#if !NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExRam.Gremlinq.Core
{
    internal static class ListExtensions
    {
        //TODO: To be worked around.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list) => CollectionsMarshal.AsSpan(list);
    }
}
#endif
