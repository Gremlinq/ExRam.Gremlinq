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
            where TBinaryMessage : IMemoryOwner<byte> => Verify(ToText<TBinaryMessage>(bytecode))
                .DontScrubGuids();

        private static string ToText<TBinaryMessage>(Bytecode bytecode)
            where TBinaryMessage : IMemoryOwner<byte>
        {
            var environment = GremlinQueryEnvironment.Invalid
                .AddGraphSonBinarySupport();

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
    }
}
