using System.Buffers;
using System.Text;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public sealed class Issue219 : VerifyBase
    {
        private readonly struct CustomMemoryOwner : IMemoryOwner<byte>
        {
            private readonly byte[]? _array;

            public CustomMemoryOwner(byte[] array)
            {
                _array = array;
            }

            public Memory<byte> Memory
            {
                get
                {
                    return _array ?? Memory<byte>.Empty;
                }
            }

            public void Dispose()
            {
                
            }
        }

        public Issue219() : base()
        {

        }

        [Fact]
        public Task Repro()
        {
            var env = GremlinQueryEnvironment.Invalid
                .UseNewtonsoftJson();

            var memory = new CustomMemoryOwner(Encoding.UTF8.GetBytes("\"2021-03-31T14:57:20.3482309Z\""));

            var token = env
                .Deserializer
                .TransformTo<JToken>()
                .From(new GraphSon3MessageBuffer(memory), env);

            return Verify(token.ToString());
        }
    }
}
