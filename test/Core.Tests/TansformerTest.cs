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
            await Verify(Transformer.Empty
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(Transformer.Empty
                .Add(Create<object, string>((serialized, env, _, recurse) => "overridden"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Base_type_in_request()
        {
            await Verify(Transformer.Empty
                .Add(Create<object, string>((serialized, env, _, recurse) => "overridden"))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Base_type_in_request_with_struct_available()
        {
            await Verify(Transformer.Empty
                .Add(Create<object, int>((serialized, env, _, recurse) => 36))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, _, recurse) => "overridden 1"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, _, recurse) => "overridden 1"))
                .Add(Create<string, string>((serialized, env, _, recurse) => "overridden 2"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, int>((serialized, env, _, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransformTo<int>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            Transformer.Empty
                .Add(Create<string, int>((serialized, env, _, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransform<int, string>(36, GremlinQueryEnvironment.Invalid, out var _)
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(Transformer.Empty
                .Add(Create<int, string>((serialized, env, _, recurse) => serialized.ToString()))
                .Add(Create<string, string>((serialized, env, _, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, _, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .Add(Create<int, string>((serialized, env, _, recurse) => serialized.ToString()))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Defer()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, _, recurse) => "overridden 1"))
                .Add(Create<string, string>((serialized, env, defer, recurse) => "overridden 2" + " " + defer.TransformTo<string>().From(serialized, env)))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Defer_3()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, defer, recurse) => "3"))
                .Add(Create<string, string>((serialized, env, defer, recurse) => "2 " + defer.TransformTo<string>().From(serialized, env)))
                .Add(Create<string, string>((serialized, env, defer, recurse) => "1 " + defer.TransformTo<string>().From(serialized, env)))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Defer_with_recurse()
        {
            await Verify(Transformer.Empty
                .Add(Create<string, string>((serialized, env, _, recurse) => serialized switch
                {
                    "deferred" => recurse.TransformTo<string>().From("recursed", env),
                    _ => throw new InvalidOperationException()
                }))
                .Add(Create<string, string>((serialized, env, defer, recurse) => serialized switch
                {
                    "serialized" => defer.TransformTo<string>().From("deferred", env),
                    "recursed" => "success",
                    _ => throw new InvalidOperationException()
                }))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public async Task Defer_with_recurse_different_types()
        {
            await Verify(Transformer.Empty
                .Add(Create<int, string>((serialized, env, _, recurse) => serialized switch
                {
                    42 => recurse.TransformTo<string>().From(DateTime.Now, env),
                    _ => throw new InvalidOperationException()
                }))
                .Add(Create<string, string>((serialized, env, defer, recurse) => serialized switch
                {
                    "serialized" => defer.TransformTo<string>().From(42, env),
                    "recursed" => "success",
                    _ => throw new InvalidOperationException()
                }))
                .Add(Create<DateTime, string>((serialized, env, defer, recurse) => "success"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));
        }
    }
}
