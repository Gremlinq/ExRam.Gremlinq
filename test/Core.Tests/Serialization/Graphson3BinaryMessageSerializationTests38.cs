using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3BinaryMessageSerializationTests38 : Graphson3BinaryMessageSerializationTests38Base, IClassFixture<Graphson3BinaryMessageSerializationTests38.Fixture>, ISourceFileNameProvider<Graphson3BinaryMessageSerializationTests38>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(env => env
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)));
        }

        public Graphson3BinaryMessageSerializationTests38(Fixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
