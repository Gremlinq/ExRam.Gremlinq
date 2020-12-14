using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal static class ImmutableStackExtensions
    {
        public static Step? TryGetSingleStep(this IImmutableStack<Step> steps)
        {
            return !steps.IsEmpty && steps.Pop(out var step).IsEmpty
                ? step
                : default;
        }

        public static Step? PeekOrDefault(this IImmutableStack<Step> steps)
        {
            return !steps.IsEmpty
                ? steps.Peek()
                : default;
        }
    }
}
