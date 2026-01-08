using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
            .TransformQuerySource(g)
            .UseNeptune<Vertex, Edge>(configurator => configurator
                .At(new Uri(Environment.GetEnvironmentVariable("Gremlinq:Neptune:Uri")!))
                .UseIAMAuthentication(iam => iam
                    .UseSigV4()
                    .WithUri(new Uri(Environment.GetEnvironmentVariable("Gremlinq:Neptune:Uri")!))
                    .WithRegion("eu-central-1")
                    .WithAccessKeyId(Environment.GetEnvironmentVariable("Gremlinq:Neptune:IAM:AccessKeyId")!)
                    .WithSecretAccessKey(Environment.GetEnvironmentVariable("Gremlinq:Neptune:IAM:SecretAccessKey")!))
                .UseNewtonsoftJson());
    }
}
