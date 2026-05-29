using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

namespace ExRam.Gremlinq.Core.Tests
{
    public class CachingGremlinQueryEnvironmentTest
    {
        [Fact]
        public void InnerEnvironment_is_exposed()
        {
            var inner = GremlinQueryEnvironment.Invalid;

            Wrap(inner)
                .InnerEnvironment
                .Should()
                .BeSameAs(inner);
        }

        [Fact]
        public void Logger_delegates_to_inner()
        {
            var logger = NullLogger.Instance;

            Wrap(GremlinQueryEnvironment.Invalid.UseLogger(logger))
                .Logger
                .Should()
                .BeSameAs(logger);
        }

        [Fact]
        public void Model_delegates_to_inner()
        {
            var model = Substitute.For<IGraphModel>();

            Wrap(GremlinQueryEnvironment.Invalid.UseModel(model))
                .Model
                .Should()
                .BeSameAs(model);
        }

        [Fact]
        public void FeatureSet_delegates_to_inner()
        {
            var env = GremlinQueryEnvironment.Invalid
                .ConfigureFeatureSet(fs => fs.ConfigureGraphFeatures(static _ => GraphFeatures.None));

            Wrap(env)
                .FeatureSet
                .GraphFeatures
                .Should()
                .Be(GraphFeatures.None);
        }

        [Fact]
        public void Serializer_delegates_to_inner()
        {
            var serializer = Substitute.For<ITransformer>();

            Wrap(GremlinQueryEnvironment.Invalid.UseSerializer(serializer))
                .Serializer
                .Should()
                .BeSameAs(serializer);
        }

        [Fact]
        public void Options_delegates_to_inner()
        {
            var env = GremlinQueryEnvironment.Invalid
                .ConfigureOptions(o => o.SetValue(GremlinqOption.Alias, "h"));

            Wrap(env)
                .Options
                .GetValue(GremlinqOption.Alias)
                .Should()
                .Be("h");
        }

        [Fact]
        public void Deserializer_delegates_to_inner()
        {
            var deserializer = Substitute.For<ITransformer>();

            Wrap(GremlinQueryEnvironment.Invalid.UseDeserializer(deserializer))
                .Deserializer
                .Should()
                .BeSameAs(deserializer);
        }

        [Fact]
        public void Debugger_delegates_to_inner()
        {
            var debugger = Substitute.For<IGremlinQueryDebugger>();

            Wrap(GremlinQueryEnvironment.Invalid.UseDebugger(debugger))
                .Debugger
                .Should()
                .BeSameAs(debugger);
        }

        [Fact]
        public void Executor_delegates_to_inner()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            Wrap(GremlinQueryEnvironment.Invalid.UseExecutor(executor))
                .Executor
                .Should()
                .BeSameAs(executor);
        }

        [Fact]
        public void NativeTypes_delegates_to_inner()
        {
            var env = GremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Add(typeof(decimal)));

            Wrap(env)
                .NativeTypes
                .Should()
                .Contain(typeof(decimal));
        }

        // ── Configure* methods ──────────────────────────────────────────────────

        [Fact]
        public void ConfigureLogger_delegates_to_inner()
        {
            var logger = NullLogger.Instance;

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureLogger(_ => logger)
                .Logger
                .Should()
                .BeSameAs(logger);
        }

        [Fact]
        public void ConfigureModel_delegates_to_inner()
        {
            var model = Substitute.For<IGraphModel>();

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureModel(_ => model)
                .Model
                .Should()
                .BeSameAs(model);
        }

        [Fact]
        public void ConfigureFeatureSet_delegates_to_inner()
        {
            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureFeatureSet(fs => fs.ConfigureGraphFeatures(static _ => GraphFeatures.None))
                .FeatureSet
                .GraphFeatures
                .Should()
                .Be(GraphFeatures.None);
        }

        [Fact]
        public void ConfigureSerializer_delegates_to_inner()
        {
            var serializer = Substitute.For<ITransformer>();

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureSerializer(_ => serializer)
                .Serializer
                .Should()
                .BeSameAs(serializer);
        }

        [Fact]
        public void ConfigureOptions_delegates_to_inner()
        {
            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureOptions(o => o.SetValue(GremlinqOption.Alias, "h"))
                .Options
                .GetValue(GremlinqOption.Alias)
                .Should()
                .Be("h");
        }

        [Fact]
        public void ConfigureDeserializer_delegates_to_inner()
        {
            var deserializer = Substitute.For<ITransformer>();

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureDeserializer(_ => deserializer)
                .Deserializer
                .Should()
                .BeSameAs(deserializer);
        }

        [Fact]
        public void ConfigureNativeTypes_delegates_to_inner()
        {
            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureNativeTypes(types => types.Add(typeof(decimal)))
                .NativeTypes
                .Should()
                .Contain(typeof(decimal));
        }

        [Fact]
        public void ConfigureDebugger_delegates_to_inner()
        {
            var debugger = Substitute.For<IGremlinQueryDebugger>();

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureDebugger(_ => debugger)
                .Debugger
                .Should()
                .BeSameAs(debugger);
        }

        [Fact]
        public void ConfigureExecutor_delegates_to_inner()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            Wrap(GremlinQueryEnvironment.Invalid)
                .ConfigureExecutor(_ => executor)
                .Executor
                .Should()
                .BeSameAs(executor);
        }

        private static ICachingGremlinQueryEnvironment Wrap(IGremlinQueryEnvironment env) => new CachingGremlinQueryEnvironmentImpl(env);
    }
}
