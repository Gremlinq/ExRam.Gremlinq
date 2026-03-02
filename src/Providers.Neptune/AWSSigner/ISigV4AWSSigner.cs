namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// An AWS SigV4 signer that can be configured with credentials and region.
    /// </summary>
    public interface ISigV4AWSSigner : IAWSSigner
    {
        /// <summary>
        /// Configures the URI used for signing by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current URI.</param>
        ISigV4AWSSigner ConfigureUri(Func<Uri, Uri> transformation);

        /// <summary>
        /// Configures the AWS region by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current region string.</param>
        ISigV4AWSSigner ConfigureRegion(Func<string, string> transformation);

        /// <summary>
        /// Configures the header cache time by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current cache time.</param>
        ISigV4AWSSigner ConfigureCacheTime(Func<TimeSpan, TimeSpan> transformation);

        /// <summary>
        /// Sets the AWS access key id.
        /// </summary>
        /// <param name="accessKeyId">The AWS access key id.</param>
        ISigV4AWSSigner WithAccessKeyId(string accessKeyId);

        /// <summary>
        /// Sets the AWS secret access key.
        /// </summary>
        /// <param name="secretAccessKey">The AWS secret access key.</param>
        ISigV4AWSSigner WithSecretAccessKey(string secretAccessKey);
    }
}
