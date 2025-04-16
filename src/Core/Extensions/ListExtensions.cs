#if !NET7_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class ListExtensions
    {
        //TODO: To be worked around.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list)
        {
            return
#if NET5_0_OR_GREATER
                System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
#else
                list.ToArray();
#endif
        }
    }
}
#endif
