using ExRam.Gremlinq.Core.Transformation;
using FluentAssertions;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TansformerTest : VerifyBase
    {
        public TansformerTest() : base()
        {

        }

        [Fact]
        public async Task Empty()
        {
            await Verify(Transformer.Identity
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, string>((serialized, env, recurse) => "overridden"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type_in_request()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, string>((serialized, env, recurse) => "overridden"))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type_in_request_with_struct_available()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, int>((serialized, env, recurse) => 36))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 1"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 1"))
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 2"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, int>((serialized, env, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransformTo<int>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            Transformer.Identity
                .Add(Create<string, int>((serialized, env, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransform<int, string>(36, GremlinQueryEnvironment.Empty, out var _)
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(Transformer.Identity
                .Add(Create<int, string>((serialized, env, recurse) => serialized.ToString()))
                .Add(Create<string, string>((serialized, env, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .Add(Create<int, string>((serialized, env, recurse) => serialized.ToString()))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }
    }
}
