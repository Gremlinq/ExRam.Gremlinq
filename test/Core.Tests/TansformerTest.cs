using ExRam.Gremlinq.Core.Transformation;
using FluentAssertions;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TansformerTest
    {
        [Fact]
        public async Task Empty() => await Verify(Transformer.Empty
           .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Base_type() => await Verify(Transformer.Empty
            .Add(Create<object, string>((_, _, _, _) => "overridden"))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Base_type_in_request() => await Verify(Transformer.Empty
            .Add(Create<object, string>((_, _, _, _) => "overridden"))
            .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Base_type_in_request_with_struct_available() => await Verify(Transformer.Empty
           .Add(Create<object, int>((_, _, _, _) => 36))
           .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Override1() => await Verify(Transformer.Empty
            .Add(Create<string, string>((_, _, _, _) => "overridden 1"))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Override2() => await Verify(Transformer.Empty
            .Add(Create<string, string>((_, _, _, _) => "overridden 1"))
            .Add(Create<string, string>((_, _, _, _) => "overridden 2"))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Recurse() => await Verify(Transformer.Empty
            .Add(Create<string, int>((_, env, _, recurse) => recurse.TryTransformTo<int>().From(36, env)))
            .TryTransformTo<int>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public void Recurse_wrong_type() => Transformer.Empty
            .Add(Create<string, int>((_, env, _, recurse) => recurse.TryTransformTo<int>().From(36, env)))
            .TryTransform<int, string>(36, GremlinQueryEnvironment.Invalid, out var _)
            .Should()
            .BeFalse();

        [Fact]
        public async Task Recurse_to_previous_override() => await Verify(Transformer.Empty
            .Add(Create<int, string>((serialized, _, _, _) => serialized.ToString()))
            .Add(Create<string, string>((serialized, env, _, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Recurse_to_later_override() => await Verify(Transformer.Empty
            .Add(Create<string, string>((serialized, env, _, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
            .Add(Create<int, string>((serialized, _, _, _) => serialized.ToString()))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Defer() => await Verify(Transformer.Empty
            .Add(Create<string, string>((_, _, _, _) => "overridden 1"))
            .Add(Create<string, string>((serialized, env, defer, _) => "overridden 2" + " " + defer.TransformTo<string>().From(serialized, env)))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Defer_3() => await Verify(Transformer.Empty
            .Add(Create<string, string>((_, _, _, _) => "3"))
            .Add(Create<string, string>((serialized, env, defer, _) => "2 " + defer.TransformTo<string>().From(serialized, env)))
            .Add(Create<string, string>((serialized, env, defer, _) => "1 " + defer.TransformTo<string>().From(serialized, env)))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Defer_3_with_recurse() => await Verify(Transformer.Empty
            .Add(Create<string, string>((serialized, env, _, recurse) => serialized switch
            {
                "deferred twice" => recurse.TransformTo<string>().From("recursed", env),
                _ => throw new InvalidOperationException()
            }))
            .Add(Create<string, string>((serialized, env, defer, _) => serialized switch
            {
                "deferred once" => defer.TransformTo<string>().From("deferred twice", env),
                _ => throw new InvalidOperationException()
            }))
            .Add(Create<string, string>((serialized, env, defer, _) => serialized switch
            {
                "serialized" => defer.TransformTo<string>().From("deferred once", env),
                "recursed" => "success",
                _ => throw new InvalidOperationException()
            }))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Defer_with_recurse() => await Verify(Transformer.Empty
            .Add(Create<string, string>((serialized, env, _, recurse) => serialized switch
            {
                "deferred" => recurse.TransformTo<string>().From("recursed", env),
                _ => throw new InvalidOperationException()
            }))
            .Add(Create<string, string>((serialized, env, defer, _) => serialized switch
            {
                "serialized" => defer.TransformTo<string>().From("deferred", env),
                "recursed" => "success",
                _ => throw new InvalidOperationException()
            }))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public async Task Defer_with_recurse_different_types() => await Verify(Transformer.Empty
            .Add(Create<int, string>((serialized, env, _, recurse) => serialized switch
            {
                42 => recurse.TransformTo<string>().From(DateTime.Now, env),
                _ => throw new InvalidOperationException()
            }))
            .Add(Create<string, string>((serialized, env, defer, _) => serialized switch
            {
                "serialized" => defer.TransformTo<string>().From(42, env),
                "recursed" => "success",
                _ => throw new InvalidOperationException()
            }))
            .Add(Create<DateTime, string>((_, _, _, _) => "success"))
            .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Invalid));

        [Fact]
        public void Struct_converter_returning_null_does_not_transform() => Transformer.Empty
            .Add(Create<string, int>((_, _, _, _) => null))
            .TryTransform<string, int>("serialized", GremlinQueryEnvironment.Invalid, out _)
            .Should()
            .BeFalse();

        [Fact]
        public void Struct_converter_returning_null_does_not_transform_to_ValueType() => Transformer.Empty
            .Add(Create<string, int>((_, _, _, _) => null))
            .TryTransform<string, ValueType>("serialized", GremlinQueryEnvironment.Invalid, out _)
            .Should()
            .BeFalse();

        [Fact]
        public void ConverterKey_equals_an_object_with_the_same_parts()
        {
            var environment = GremlinQueryEnvironment.Invalid;
            var key = new Transformer.ConverterKey(environment, typeof(string), typeof(string), typeof(object));

            key.Equals((object)new Transformer.ConverterKey(environment, typeof(string), typeof(string), typeof(object))).Should().BeTrue();
            key.Equals((object)new Transformer.ConverterKey(environment.ConfigureOptions(options => options), typeof(string), typeof(string), typeof(object))).Should().BeFalse();
            key.Equals("key").Should().BeFalse();
        }
    }
}
