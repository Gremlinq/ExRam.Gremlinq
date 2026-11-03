using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// What the GraphSON 3 writer sends to a server that understands TinkerPop 3.7.7 to 3.8.2. Such a server understands every
    /// form before 3.8, so every fact answers like <see cref="Graphson3BinaryMessageSerializationTests"/>.
    /// </summary>
    public sealed class Graphson3BinaryMessageSerializationTests37To38 : Graphson3BinaryMessageSerializationTestsBase, IClassFixture<Graphson3BinaryMessageSerializationTests37To38.Fixture>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(env => env
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2)));
        }

        public Graphson3BinaryMessageSerializationTests37To38(Fixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }
    }
}
