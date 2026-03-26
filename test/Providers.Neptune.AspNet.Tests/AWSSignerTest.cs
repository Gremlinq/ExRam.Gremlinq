using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using Amazon.Runtime.Credentials;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Neptune.AspNet.Tests
{
    public class AWSSignerTest
    {
        [Fact]
        public Task Disabled() => Verify(new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Neptune:IAM:Disabled", "true" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>()
                .UseIAMAuthentication())
            .BuildServiceProvider()
            .GetRequiredService<IAWSSigner>()
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public void Insufficient_1() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection([])
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>()
                .UseIAMAuthentication())
            .BuildServiceProvider()
            .GetRequiredService<IAWSSigner>()
            .Invoking(_ => _
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")))
            .Should()
            .Throw<InvalidOperationException>();

        [Fact]
        public void Insufficient_2() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>()
                .UseIAMAuthentication())
            .BuildServiceProvider()
            .GetRequiredService<IAWSSigner>()
            .Invoking(_ => _
                .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")))
            .Should()
            .Throw<InvalidOperationException>();

        [Fact]
        public Task Minimum() => Verify(new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                    { "Gremlinq:Neptune:IAM:AccessKeyId", "accessKey" },
                    { "Gremlinq:Neptune:IAM:SecretAccessKey", "secretAccessKey" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>()
                .UseIAMAuthentication())
            .BuildServiceProvider()
            .GetRequiredService<IAWSSigner>()
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Minimum_same_Uri() => Verify(new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                    { "Gremlinq:Neptune:IAM:Uri", "ws://localhost:8182" },
                    { "Gremlinq:Neptune:IAM:AccessKeyId", "accessKey" },
                    { "Gremlinq:Neptune:IAM:SecretAccessKey", "secretAccessKey" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>()
                .UseIAMAuthentication())
            .BuildServiceProvider()
            .GetRequiredService<IAWSSigner>()
            .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Minimum_same_Uri_explicit_path() => Verify(new ServiceCollection()
           .AddSingleton<IConfiguration>(new ConfigurationBuilder()
               .AddInMemoryCollection(new Dictionary<string, string?>
               {
                    { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                    { "Gremlinq:Neptune:IAM:Uri", "ws://localhost:8182/gremlin" },
                    { "Gremlinq:Neptune:IAM:AccessKeyId", "accessKey" },
                    { "Gremlinq:Neptune:IAM:SecretAccessKey", "secretAccessKey" }
               })
               .Build())
           .AddGremlinq(setup => setup
               .UseNeptune<Vertex, Edge>()
               .UseIAMAuthentication())
           .BuildServiceProvider()
           .GetRequiredService<IAWSSigner>()
           .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task Minimum_same_Uri_different_path() => Verify(new ServiceCollection()
           .AddSingleton<IConfiguration>(new ConfigurationBuilder()
               .AddInMemoryCollection(new Dictionary<string, string?>
               {
                    { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                    { "Gremlinq:Neptune:IAM:Uri", "ws://localhost:8182/streams" },
                    { "Gremlinq:Neptune:IAM:AccessKeyId", "accessKey" },
                    { "Gremlinq:Neptune:IAM:SecretAccessKey", "secretAccessKey" }
               })
               .Build())
           .AddGremlinq(setup => setup
               .UseNeptune<Vertex, Edge>()
               .UseIAMAuthentication())
           .BuildServiceProvider()
           .GetRequiredService<IAWSSigner>()
           .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));

        [Fact]
        public Task UseIAMAuthentication_with_IdentityResolver_from_environment_variables()
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "envAccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "envSecretAccessKey");

            try
            {
                return Verify(new ServiceCollection()
                    .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                        })
                        .Build())
                    .AddGremlinq(setup => setup
                        .UseNeptune<Vertex, Edge>()
                        .UseIAMAuthentication(new DefaultAWSCredentialsIdentityResolver()))
                    .BuildServiceProvider()
                    .GetRequiredService<IAWSSigner>()
                    .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
            }
            finally
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
            }
        }

        [Fact]
        public Task UseIAMAuthentication_with_IdentityResolver_from_environment_variables_with_region()
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "envAccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "envSecretAccessKey");

            try
            {
                return Verify(new ServiceCollection()
                    .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "Gremlinq:Neptune:Uri", "ws://localhost:8182" },
                            { "Gremlinq:Neptune:IAM:Region", "eu-west-1" },
                        })
                        .Build())
                    .AddGremlinq(setup => setup
                        .UseNeptune<Vertex, Edge>()
                        .UseIAMAuthentication(new DefaultAWSCredentialsIdentityResolver()))
                    .BuildServiceProvider()
                    .GetRequiredService<IAWSSigner>()
                    .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00")));
            }
            finally
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
            }
        }

        [Fact]
        public void UseIAMAuthentication_with_IdentityResolver_Disabled()
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "envAccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "envSecretAccessKey");

            try
            {
                new ServiceCollection()
                    .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "Gremlinq:Neptune:IAM:Disabled", "true" },
                        })
                        .Build())
                    .AddGremlinq(setup => setup
                        .UseNeptune<Vertex, Edge>()
                        .UseIAMAuthentication(new DefaultAWSCredentialsIdentityResolver()))
                    .BuildServiceProvider()
                    .GetRequiredService<IAWSSigner>()
                    .GetIAMHeaders(DateTimeOffset.Parse("01.01.2021 09:00"))
                    .Should()
                    .BeEmpty();
            }
            finally
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
            }
        }
    }
}
