using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    // What the GraphSON 2 writer sends where TinkerPop 3.8 writes a type in another form than 3.7,
    // for a server from TinkerPop 3.8 on. A range that spans both is sent the 3.7 form, the snapshot
    // of Graphson2BinaryMessageSerializationTest.
    public class Graphson2BinaryMessageSerializationTest38 : GremlinqTestBase, ISourceFileNameProvider<Graphson2BinaryMessageSerializationTest38>
    {
        private readonly IGremlinQuerySource _g;

        public Graphson2BinaryMessageSerializationTest38() : base(new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                    .AddGraphSonBinarySupport()
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)));
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // TinkerPop's Byte is signed (TINKERPOP-3161): an sbyte goes out as gx:Byte, a byte as a bare number.
        [Fact]
        public Task Inject_byte() => _g
            .Inject((byte)200)
            .Verify();

        [Fact]
        public Task Inject_sbyte() => _g
            .Inject((sbyte)-56)
            .Verify();

        [Fact]
        public Task Inject_byte_from_TinkerPop_3_7_to_3_8() => _g
            .ConfigureEnvironment(env => env
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2)))
            .Inject((byte)200)
            .Verify()
            .UseSnapshotDirectoryAndNameOf<Graphson2BinaryMessageSerializationTest>()
            .UseMethodName(nameof(Graphson2BinaryMessageSerializationTest.Inject_byte));

        [Fact]
        public Task Inject_sbyte_from_TinkerPop_3_7_to_3_8() => _g
            .ConfigureEnvironment(env => env
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2)))
            .Inject((sbyte)-56)
            .Verify()
            .UseSnapshotDirectoryAndNameOf<Graphson2BinaryMessageSerializationTest>()
            .UseMethodName(nameof(Graphson2BinaryMessageSerializationTest.Inject_sbyte));

        // A DateTimeOffset still goes out as g:Date, as to a server below 3.8.
        [Fact]
        public Task Inject_DateTimeOffset() => _g
            .Inject(new DateTimeOffset(2023, 8, 2, 2, 0, 0, TimeSpan.FromHours(2)))
            .Verify();
    }
}
