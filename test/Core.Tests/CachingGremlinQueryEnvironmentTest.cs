using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

namespace ExRam.Gremlinq.Core.Tests
{
    public class CachingGremlinQueryEnvironmentTest
    {
        [Fact]
        public void Logger_delegates_to_inner()
        {
            var logger = NullLogger.Instance;

            CachingGremlinQueryEnvironment.Invalid
                .UseLogger(logger)
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
            CachingGremlinQueryEnvironment.Invalid
                .ConfigureFeatureSet(fs => fs.ConfigureGraphFeatures(static _ => GraphFeatures.None))
                .FeatureSet
                .GraphFeatures
                .Should()
                .Be(GraphFeatures.None);
        }

        [Fact]
        public void Serializer_delegates_to_inner()
        {
            var serializer = Substitute.For<ITransformer>();

            CachingGremlinQueryEnvironment.Invalid
                .UseSerializer(serializer)
                .Serializer
                .Should()
                .BeSameAs(serializer);
        }

        [Fact]
        public void Options_delegates_to_inner()
        {
            CachingGremlinQueryEnvironment.Invalid
                .ConfigureOptions(o => o.SetValue(GremlinqOption.Alias, "h"))
                .Options
                .GetValue(GremlinqOption.Alias)
                .Should()
                .Be("h");
        }

        [Fact]
        public void Deserializer_delegates_to_inner()
        {
            var deserializer = Substitute.For<ITransformer>();

            CachingGremlinQueryEnvironment.Invalid
                .UseDeserializer(deserializer)
                .Deserializer
                .Should()
                .BeSameAs(deserializer);
        }

        [Fact]
        public void Debugger_delegates_to_inner()
        {
            var debugger = Substitute.For<IGremlinQueryDebugger>();

            CachingGremlinQueryEnvironment.Invalid
                .UseDebugger(debugger)
                .Debugger
                .Should()
                .BeSameAs(debugger);
        }

        [Fact]
        public void Executor_delegates_to_inner()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            CachingGremlinQueryEnvironment.Invalid
                .UseExecutor(executor)
                .Executor
                .Should()
                .BeSameAs(executor);
        }

        [Fact]
        public void NativeTypes_delegates_to_inner()
        {
            CachingGremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Add(typeof(decimal)))
                .NativeTypes
                .Should()
                .Contain(typeof(decimal));
        }

        // ── Configure* methods ──────────────────────────────────────────────────

        [Fact]
        public void ConfigureLogger_delegates_to_inner()
        {
            var logger = NullLogger.Instance;

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureLogger(_ => logger)
                .Logger
                .Should()
                .BeSameAs(logger);
        }

        [Fact]
        public void ConfigureModel_delegates_to_inner()
        {
            var model = Substitute.For<IGraphModel>();

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureModel(_ => model)
                .Model
                .Should()
                .BeSameAs(model);
        }

        [Fact]
        public void ConfigureFeatureSet_delegates_to_inner()
        {
            CachingGremlinQueryEnvironment.Invalid
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

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureSerializer(_ => serializer)
                .Serializer
                .Should()
                .BeSameAs(serializer);
        }

        [Fact]
        public void ConfigureOptions_delegates_to_inner()
        {
            CachingGremlinQueryEnvironment.Invalid
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

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureDeserializer(_ => deserializer)
                .Deserializer
                .Should()
                .BeSameAs(deserializer);
        }

        [Fact]
        public void ConfigureNativeTypes_delegates_to_inner()
        {
            CachingGremlinQueryEnvironment.Invalid
                .ConfigureNativeTypes(types => types.Add(typeof(decimal)))
                .NativeTypes
                .Should()
                .Contain(typeof(decimal));
        }

        [Fact]
        public void ConfigureDebugger_delegates_to_inner()
        {
            var debugger = Substitute.For<IGremlinQueryDebugger>();

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureDebugger(_ => debugger)
                .Debugger
                .Should()
                .BeSameAs(debugger);
        }

        [Fact]
        public void ConfigureExecutor_delegates_to_inner()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            CachingGremlinQueryEnvironment.Invalid
                .ConfigureExecutor(_ => executor)
                .Executor
                .Should()
                .BeSameAs(executor);
        }

        // ── Cache-sharing: non-model Configure* return ICachingGremlinQueryEnvironment ──

        [Fact]
        public void ConfigureLogger_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureLogger(_ => NullLogger.Instance).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureFeatureSet_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureFeatureSet(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureSerializer_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureSerializer(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureOptions_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureOptions(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureDeserializer_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureDeserializer(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureNativeTypes_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureNativeTypes(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureDebugger_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureDebugger(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureExecutor_returns_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureExecutor(x => x).Should().BeAssignableTo<ICachingGremlinQueryEnvironment>();

        [Fact]
        public void ConfigureModel_does_not_return_ICachingGremlinQueryEnvironment()
            => CachingGremlinQueryEnvironment.Invalid.ConfigureModel(x => x).Should().NotBeAssignableTo<ICachingGremlinQueryEnvironment>();

        private static ICachingGremlinQueryEnvironment Wrap(IGremlinQueryEnvironment env) => new CachingGremlinQueryEnvironment(env);
    }
}
