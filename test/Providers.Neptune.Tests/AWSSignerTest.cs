namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class AWSSignerTest : VerifyBase
    {
        private readonly IAWSSigner _signer;

        public AWSSignerTest() : base()
        {
            _signer = AWSSigner.EmptyV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithAccessKeyId("accessKeyId")
                .WithSecretAccessKey("secretAccessKey");
        }

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
    }
}
