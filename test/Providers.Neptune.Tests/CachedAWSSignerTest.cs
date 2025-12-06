namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class CachedAWSSignerTest : VerifyBase
    {
        private readonly IAWSSigner _signer;

        public CachedAWSSignerTest() : base()
        {
            _signer = AWSSigner.EmptySigV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithAccessKeyId("accessKeyId")
                .WithSecretAccessKey("secretAccessKey")
                .ConfigureCacheTime(_ => TimeSpan.FromMinutes(10));
        }

        [Fact]
        public void Headers_are_cached_1()
        {
            var signature1 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"));
            var signature2 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:06"));

            Assert.Equal(signature1, signature2);
        }

        [Fact]
        public void Headers_are_cached_within_time_window()
        {
            var signature1 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"));
            var signature2 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:06"));

            Assert.Equal(signature1, signature2);
        }

        [Fact]
        public void Headers_are_not_cached_outside_time_window()
        {
            var signature1 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"));
            var signature2 = _signer.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:11"));

            Assert.NotEqual(signature1, signature2);
        }
    }
}
