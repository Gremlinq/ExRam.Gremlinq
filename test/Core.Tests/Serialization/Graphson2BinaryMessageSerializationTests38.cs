using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2BinaryMessageSerializationTests38 : Graphson2BinaryMessageSerializationTests38Base, IClassFixture<Graphson2BinaryMessageSerializationTests38.Fixture>, ISourceFileNameProvider<Graphson2BinaryMessageSerializationTests38>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(env => env
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)));
        }

        public Graphson2BinaryMessageSerializationTests38(Fixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
