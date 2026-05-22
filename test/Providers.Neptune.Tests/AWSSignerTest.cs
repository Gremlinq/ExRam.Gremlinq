using Amazon.Runtime;
using Amazon.Runtime.Credentials;

using FluentAssertions;

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

        [Fact]
        public Task WithCacheTime()
        {
            return Verify(_signer
                .WithCacheTime(TimeSpan.FromMinutes(10))
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
        }

        [Fact]
        public Task Sign_HttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://some.host.com/gremlin");

            _signer.Sign(request, DateTimeOffset.Parse("01.01.2021 09:00"));

            return Verify(request.Headers
                .ToDictionary(h => h.Key, h => string.Join(", ", h.Value)));
        }

        [Fact]
        public void Sign_HttpRequestMessage_non_GET_throws()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://some.host.com/gremlin");

            _signer
                .Invoking(s => s.Sign(request, DateTimeOffset.Parse("01.01.2021 09:00")))
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public Task Sign_HttpHeaders()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://some.host.com/gremlin");

            _signer.Sign(request.Headers, DateTimeOffset.Parse("01.01.2021 09:00"));

            return Verify(request.Headers
                .ToDictionary(h => h.Key, h => string.Join(", ", h.Value)));
        }

        [Fact]
        public void Headers_dictionary_members()
        {
            var headers = _signer
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"));

            headers.Count.Should().Be(4);
            headers.Keys.Should().HaveCount(4);
            headers.Values.Should().HaveCount(4);
            headers.ContainsKey("host").Should().BeTrue();
            headers.ContainsKey("nonexistent").Should().BeFalse();
            headers["host"].Should().Be("some.host.com");
            headers.TryGetValue("x-amz-date", out var dateValue).Should().BeTrue();
            dateValue.Should().NotBeNullOrEmpty();
            headers.TryGetValue("nonexistent", out _).Should().BeFalse();

            headers
                .Invoking(h => _ = h["nonexistent"])
                .Should()
                .Throw<KeyNotFoundException>();

            ((System.Collections.IEnumerable)headers).GetEnumerator()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void Disabled_signer_returns_empty_headers()
        {
            AWSSigner.Disabled
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"))
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Disabled_signer_UseSigV4_returns_signer()
        {
            AWSSigner.Disabled
                .UseSigV4()
                .Should()
                .BeAssignableTo<ISigV4AWSSigner>();
        }

        [Fact]
        public void Missing_AccessKeyId_throws()
        {
            AWSSigner.EmptySigV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithSecretAccessKey("secretAccessKey")
                .Invoking(s => s.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")))
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*AccessKeyId*");
        }

        [Fact]
        public void Missing_SecretAccessKey_throws()
        {
            AWSSigner.EmptySigV4
                .ConfigureUri(_ => new Uri("http://some.host.com"))
                .ConfigureRegion(_ => "eu-central-1")
                .WithAccessKeyId("accessKeyId")
                .Invoking(s => s.GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")))
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*SecretAccessKey*");
        }
    }
}
