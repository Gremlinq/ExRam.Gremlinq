using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryEnvironmentTest
    {
        [Fact]
        public void UseModel()
        {
            var model = Substitute.For<IGraphModel>();

            GremlinQueryEnvironment.Invalid
                .UseModel(model)
                .Model
                .Should()
                .BeSameAs(model);
        }

        [Fact]
        public void UseLogger()
        {
            var logger = NullLogger.Instance;

            GremlinQueryEnvironment.Invalid
                .UseLogger(logger)
                .Logger
                .Should()
                .BeSameAs(logger);
        }

        [Fact]
        public void UseSerializer()
        {
            var serializer = Substitute.For<ITransformer>();

            GremlinQueryEnvironment.Invalid
                .UseSerializer(serializer)
                .Serializer
                .Should()
                .BeSameAs(serializer);
        }

        [Fact]
        public void UseDeserializer()
        {
            var deserializer = Substitute.For<ITransformer>();

            GremlinQueryEnvironment.Invalid
                .UseDeserializer(deserializer)
                .Deserializer
                .Should()
                .BeSameAs(deserializer);
        }

        [Fact]
        public void UseExecutor()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            GremlinQueryEnvironment.Invalid
                .UseExecutor(executor)
                .Executor
                .Should()
                .BeSameAs(executor);
        }

        [Fact]
        public void UseDebugger()
        {
            var debugger = Substitute.For<IGremlinQueryDebugger>();

            GremlinQueryEnvironment.Invalid
                .UseDebugger(debugger)
                .Debugger
                .Should()
                .BeSameAs(debugger);
        }

        [Fact]
        public void ConfigureOptions()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureOptions(options => options.SetValue(GremlinqOption.Alias, "h"))
                .Options
                .GetValue(GremlinqOption.Alias)
                .Should()
                .Be("h");
        }

        [Fact]
        public void ConfigureFeatureSet()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureFeatureSet(fs => fs.ConfigureGraphFeatures(static _ => GraphFeatures.None))
                .FeatureSet
                .GraphFeatures
                .Should()
                .Be(GraphFeatures.None);
        }

        [Fact]
        public void ConfigureNativeTypes()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Add(typeof(decimal)))
                .NativeTypes
                .Should()
                .Contain(typeof(decimal));
        }

        [Fact]
        public void SupportsType_native_type()
        {
            GremlinQueryEnvironment.Invalid
                .SupportsType(typeof(int))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void SupportsType_unsupported_type()
        {
            GremlinQueryEnvironment.Invalid
                .SupportsType(typeof(Uri))
                .Should()
                .BeFalse();
        }

        // A type the environment cannot store natively still counts as supported when there is a
        // stand-in it can store: a TimeSpan as its double, a byte[] as its string. Neptune removes
        // both from its native types and JanusGraph removes byte[], so for them these conversions
        // are the answer rather than a corner - and each holds only while its stand-in is native.
        [Fact]
        public void SupportsType_TimeSpan_as_double()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Remove(typeof(TimeSpan)))
                .SupportsType(typeof(TimeSpan))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void SupportsType_TimeSpan_without_double()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types
                    .Remove(typeof(TimeSpan))
                    .Remove(typeof(double)))
                .SupportsType(typeof(TimeSpan))
                .Should()
                .BeFalse();
        }

        [Fact]
        public void SupportsType_byte_array_as_string()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Remove(typeof(byte[])))
                .SupportsType(typeof(byte[]))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void SupportsType_byte_array_without_string()
        {
            GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types
                    .Remove(typeof(byte[]))
                    .Remove(typeof(string)))
                .SupportsType(typeof(byte[]))
                .Should()
                .BeFalse();
        }
    }
}
