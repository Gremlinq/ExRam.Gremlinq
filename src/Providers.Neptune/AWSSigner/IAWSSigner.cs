namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Provides AWS IAM authentication headers for signing requests.
    /// </summary>
    public interface IAWSSigner
    {
        /// <summary>
        /// Gets the IAM authentication headers for the specified time.
        /// </summary>
        /// <param name="time">The timestamp to use for signing. Defaults to <see cref="DateTimeOffset.UtcNow"/>.</param>
        IReadOnlyDictionary<string, string> GetIAMHeaders(DateTimeOffset? time = null);
    }
}
