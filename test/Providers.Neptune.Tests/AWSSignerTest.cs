using Amazon.Runtime;
using Amazon.Runtime.Credentials;
using Amazon.Runtime.Identity;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class AWSSignerTest : VerifyBase
    {
        private readonly ISigV4AWSSigner _signer;

        public AWSSignerTest() : base()
        {
            _signer = AWSSigner.EmptySigV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithAccessKeyId("accessKeyId")
                .WithSecretAccessKey("secretAccessKey");
        }

        [Fact]
        public Task Neptune_EuCentral_1_implicit_path() => Verify(_signer
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Neptune_EuCentral_1_with_explicit_path() => Verify(_signer
            .ConfigureUri(_ => new Uri("http://some.host.com/specificPath"))
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Neptune_EuCentral_1() => Verify(_signer
            .ConfigureUri(uri => new UriBuilder(uri) { Path = "/gremlin" }.Uri)
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Neptune_EuCentral_1_with_headers() => Verify(_signer
            .ConfigureUri(uri => new UriBuilder(uri) { Path = "/gremlin/stream", Query = "iteratorType=type&limit=1&commitNum=1&opNum=1" }.Uri)
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task WithCredentialsFrom_IdentityResolver()
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "envAccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "envSecretAccessKey");

            try
            {
                return Verify(AWSSigner.EmptySigV4
                    .ConfigureUri(_ => new Uri("http://some.host.com"))
                    .ConfigureRegion(_ => "eu-central-1")
                    .WithCredentialsFrom(new DefaultAWSCredentialsIdentityResolver())
                    .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
            }
            finally
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
            }
        }

        [Fact]
        public Task WithCredentialsFromDefaultAWSCredentialsIdentityResolver()
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "envAccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "envSecretAccessKey");

            try
            {
                return Verify(AWSSigner.EmptySigV4
                    .ConfigureUri(_ => new Uri("http://some.host.com"))
                    .ConfigureRegion(_ => "eu-central-1")
                    .WithCredentialsFromDefaultAWSCredentialsIdentityResolver()
                    .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
            }
            finally
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
            }
        }

        [Fact]
        public Task WithCredentials_AWSCredentials()
        {
            return Verify(AWSSigner.EmptySigV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithCredentials(new BasicAWSCredentials("basicAccessKey", "basicSecretKey"))
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
        }
    }
}
