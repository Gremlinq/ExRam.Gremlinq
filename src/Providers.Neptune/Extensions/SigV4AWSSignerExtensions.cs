using Amazon.Runtime;
using Amazon.Runtime.Credentials;
using Amazon.Runtime.Identity;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Provides extension methods on <see cref="ISigV4AWSSigner"/> for configuring
    /// credentials from the AWS SDK credential types.
    /// </summary>
    public static class SigV4AWSSignerExtensions
    {
        /// <summary>
        /// Sets the AWS credentials from an <see cref="AWSCredentials"/> instance by resolving them synchronously.
        /// </summary>
        /// <param name="signer">The signer to configure.</param>
        /// <param name="credentials">The AWS credentials to resolve and apply.</param>
        public static ISigV4AWSSigner WithCredentials(this ISigV4AWSSigner signer, AWSCredentials credentials)
        {
            ArgumentNullException.ThrowIfNull(signer);
            ArgumentNullException.ThrowIfNull(credentials);

            var immutableCredentials = credentials
                .GetCredentials();

            return signer
                .WithAccessKeyId(immutableCredentials.AccessKey)
                .WithSecretAccessKey(immutableCredentials.SecretKey);
        }

        /// <summary>
        /// Sets the AWS credentials by resolving them from an <see cref="IIdentityResolver{T}"/> for <see cref="AWSCredentials"/>.
        /// </summary>
        /// <param name="signer">The signer to configure.</param>
        /// <param name="identityResolver">The identity resolver to use for obtaining credentials.</param>
        /// <param name="clientConfig">Optional AWS client configuration. The AWSSDK.Core library does not annotate this parameter as nullable, but it is effectively optional.</param>
        public static ISigV4AWSSigner WithCredentials(this ISigV4AWSSigner signer, IIdentityResolver<AWSCredentials> identityResolver, IClientConfig? clientConfig = null)
        {
            ArgumentNullException.ThrowIfNull(signer);
            ArgumentNullException.ThrowIfNull(identityResolver);

            return signer
                .WithCredentials(identityResolver.ResolveIdentity(clientConfig!));
        }

        /// <summary>
        /// Sets the AWS credentials by resolving them from <see cref="DefaultAWSCredentialsIdentityResolver"/>.
        /// This uses the AWS SDK's default credential resolution chain (environment variables,
        /// AWS profiles, IMDS, ECS task roles, etc.).
        /// </summary>
        /// <param name="signer">The signer to configure.</param>
        /// <param name="clientConfig">Optional AWS client configuration.</param>
        public static ISigV4AWSSigner WithDefaultAWSCredentials(this ISigV4AWSSigner signer, IClientConfig? clientConfig = null)
        {
            ArgumentNullException.ThrowIfNull(signer);

            return signer
                .WithCredentials(new DefaultAWSCredentialsIdentityResolver(), clientConfig);
        }
    }
}
