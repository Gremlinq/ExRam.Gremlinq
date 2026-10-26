using System.Buffers;
using System.Text;

using ExRam.Gremlinq.Core;

using FluentAssertions;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using Gremlin.Net.Process.Traversal.Strategy.Verification;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class GraphSonBinarySupportTest
    {
        private static readonly Guid RequestId = Guid.Parse("12345678-9012-3456-7890-123456789012");

        [Fact]
        public Task GraphSon2_withStrategies() => VerifyMessage<GraphSon2BinaryMessage>(GetWithStrategiesBytecode());

        [Fact]
        public Task GraphSon3_withStrategies() => VerifyMessage<GraphSon3BinaryMessage>(GetWithStrategiesBytecode());

        [Fact]
        public Task GraphSon2_withoutStrategies() => VerifyMessage<GraphSon2BinaryMessage>(GetWithoutStrategiesBytecode());

        [Fact]
        public Task GraphSon3_withoutStrategies() => VerifyMessage<GraphSon3BinaryMessage>(GetWithoutStrategiesBytecode());

        // A server from TinkerPop 3.8 on is sent a strategy as {"fqcn":…,"conf":{…}} and a strategy
        // type as g:Class, as Gremlin.Net 3.8 writes them.
        [Fact]
        public Task GraphSon2_withStrategies_from_TinkerPop_3_8() => VerifyMessage<GraphSon2BinaryMessage>(GetWithStrategiesBytecode(), FromTinkerPop38);

        [Fact]
        public Task GraphSon3_withStrategies_from_TinkerPop_3_8() => VerifyMessage<GraphSon3BinaryMessage>(GetWithStrategiesBytecode(), FromTinkerPop38);

        [Fact]
        public Task GraphSon2_withoutStrategies_from_TinkerPop_3_8() => VerifyMessage<GraphSon2BinaryMessage>(GetWithoutStrategiesBytecode(), FromTinkerPop38);

        [Fact]
        public Task GraphSon3_withoutStrategies_from_TinkerPop_3_8() => VerifyMessage<GraphSon3BinaryMessage>(GetWithoutStrategiesBytecode(), FromTinkerPop38);

        // A range that starts below TinkerPop 3.8 is sent strategies and strategy types in the form
        // before 3.8.
        [Fact]
        public Task GraphSon2_withStrategies_from_TinkerPop_3_7_to_3_8() => VerifyMessage<GraphSon2BinaryMessage>(GetWithStrategiesBytecode(), FromTinkerPop37To38)
            .UseMethodName(nameof(GraphSon2_withStrategies))
            .DisableRequireUniquePrefix();

        [Fact]
        public Task GraphSon3_withStrategies_from_TinkerPop_3_7_to_3_8() => VerifyMessage<GraphSon3BinaryMessage>(GetWithStrategiesBytecode(), FromTinkerPop37To38)
            .UseMethodName(nameof(GraphSon3_withStrategies))
            .DisableRequireUniquePrefix();

        [Fact]
        public Task GraphSon2_withoutStrategies_from_TinkerPop_3_7_to_3_8() => VerifyMessage<GraphSon2BinaryMessage>(GetWithoutStrategiesBytecode(), FromTinkerPop37To38)
            .UseMethodName(nameof(GraphSon2_withoutStrategies))
            .DisableRequireUniquePrefix();

        [Fact]
        public Task GraphSon3_withoutStrategies_from_TinkerPop_3_7_to_3_8() => VerifyMessage<GraphSon3BinaryMessage>(GetWithoutStrategiesBytecode(), FromTinkerPop37To38)
            .UseMethodName(nameof(GraphSon3_withoutStrategies))
            .DisableRequireUniquePrefix();

        [Fact]
        public void Type_without_instance_in_withoutStrategies_cannot_be_written()
        {
            var bytecode = new Bytecode();

            bytecode.AddSource("withoutStrategies", typeof(int?));
            bytecode.AddStep("V");

            ((Action)(() => ToText<GraphSon3BinaryMessage>(bytecode)))
                .Should()
                .Throw<IOException>();
        }

        [Fact]
        public void Type_without_instance_in_withoutStrategies_cannot_be_written_from_TinkerPop_3_8()
        {
            var bytecode = new Bytecode();

            bytecode.AddSource("withoutStrategies", typeof(int?));
            bytecode.AddStep("V");

            ((Action)(() => ToText<GraphSon3BinaryMessage>(bytecode, FromTinkerPop38)))
                .Should()
                .Throw<IOException>();
        }

        private static Bytecode GetWithStrategiesBytecode()
        {
            var bytecode = new Bytecode();

            bytecode.AddSource("withStrategies", new PartitionStrategy(partitionKey: "tenant", writePartition: "a", includeMetaProperties: true));
            bytecode.AddStep("V");

            return bytecode;
        }

        private static Bytecode GetWithoutStrategiesBytecode()
        {
            var bytecode = new Bytecode();

            bytecode.AddSource("withoutStrategies", typeof(ReadOnlyStrategy));
            bytecode.AddStep("V");

            return bytecode;
        }

        private static SettingsTask VerifyMessage<TBinaryMessage>(Bytecode bytecode)
            where TBinaryMessage : IMemoryOwner<byte> => VerifyMessage<TBinaryMessage>(bytecode, static environment => environment);

        private static SettingsTask VerifyMessage<TBinaryMessage>(Bytecode bytecode, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            where TBinaryMessage : IMemoryOwner<byte> => Verify(ToText<TBinaryMessage>(bytecode, environmentTransformation))
                .DontScrubGuids();

        private static string ToText<TBinaryMessage>(Bytecode bytecode)
            where TBinaryMessage : IMemoryOwner<byte> => ToText<TBinaryMessage>(bytecode, static environment => environment);

        private static string ToText<TBinaryMessage>(Bytecode bytecode, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            where TBinaryMessage : IMemoryOwner<byte>
        {
            var environment = environmentTransformation(GremlinQueryEnvironment.Invalid
                .AddGraphSonBinarySupport());

            var requestMessage = RequestMessage
                .Build(Tokens.OpsBytecode)
                .Processor(Tokens.ProcessorTraversal)
                .OverrideRequestId(RequestId)
                .AddArgument(Tokens.ArgsGremlin, bytecode)
                .AddArgument(Tokens.ArgsAliases, new Dictionary<string, string> { { "g", "g" } })
                .Create();

            using (var binaryMessage = environment.Serializer
                .TransformTo<TBinaryMessage>()
                .From(requestMessage, environment))
            {
                return Encoding.UTF8.GetString(binaryMessage.Memory.Span);
            }
        }

        private static IGremlinQueryEnvironment FromTinkerPop38(IGremlinQueryEnvironment environment) => environment
            .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2));

        private static IGremlinQueryEnvironment FromTinkerPop37To38(IGremlinQueryEnvironment environment) => environment
            .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2));
    }
}
