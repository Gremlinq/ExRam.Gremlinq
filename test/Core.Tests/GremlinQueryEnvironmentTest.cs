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
    }
}
