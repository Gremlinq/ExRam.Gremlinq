namespace ExRam.Gremlinq.Providers.Core
{
    internal static class CancellationTokenExtensions
    {
        // Links two tokens only when both can be cancelled; otherwise the one that can is the linked token, and
        // there is nothing to dispose.
        public static IDisposable? TryLink(this CancellationToken token, CancellationToken other, out CancellationToken linked)
        {
            if (token.CanBeCanceled && other.CanBeCanceled)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token, other);

                linked = cts.Token;
                return cts;
            }

            linked = token.CanBeCanceled
                ? token
                : other;

            return null;
        }
    }
}
