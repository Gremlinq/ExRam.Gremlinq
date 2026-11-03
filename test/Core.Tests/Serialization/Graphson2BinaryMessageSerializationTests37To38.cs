using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// What the GraphSON 2 writer sends to a server that understands TinkerPop 3.7.7 to 3.8.2. Such a server understands every
    /// form before 3.8, so every fact answers like <see cref="Graphson2BinaryMessageSerializationTests"/>.
    /// </summary>
    public sealed class Graphson2BinaryMessageSerializationTests37To38 : Graphson2BinaryMessageSerializationTestsBase, IClassFixture<Graphson2BinaryMessageSerializationTests37To38.Fixture>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(env => env
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2)));
        }

        public Graphson2BinaryMessageSerializationTests37To38(Fixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
        }
    }
}
