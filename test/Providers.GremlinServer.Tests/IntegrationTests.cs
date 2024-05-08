﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

using FluentAssertions;

using Newtonsoft.Json.Linq;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest]
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public IntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }

        [Fact]
        public async Task FirstAsync() => (await _g
            .Inject(42)
            .FirstAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task FirstAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .FirstAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task FirstOrDefaultAsync() => (await _g
            .Inject(42)
            .FirstOrDefaultAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task FirstOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .FirstOrDefaultAsync())
                .Should()
                .Be(0);

        [Fact]
        public async Task SingleAsync() => (await _g
           .Inject(42)
           .SingleAsync())
               .Should()
               .Be(42);

        [Fact]
        public async Task SingleAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .SingleAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task SingleAsync_two_elements() => await _g
            .Inject(42, 43)
            .Awaiting(_ => _
                .SingleAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task SingleOrDefaultAsync() => (await _g
            .Inject(42)
            .SingleOrDefaultAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task SingleOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .SingleOrDefaultAsync())
                .Should()
                .Be(0);

        [Fact]
        public async Task SingleOrDefaultAsync_two_elements() => await _g
            .Inject(42, 43)
            .Awaiting(_ => _
                .SingleOrDefaultAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task LastAsync() => (await _g
           .Inject(42, 43)
           .LastAsync())
               .Should()
               .Be(43);

        [Fact]
        public async Task LastAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .LastAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task LastOrDefaultAsync() => (await _g
            .Inject(42, 43)
            .LastOrDefaultAsync())
                .Should()
                .Be(43);

        [Fact]
        public async Task LastOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .LastOrDefaultAsync())
                .Should()
                .Be(0);

        [Fact]
        public async Task Deserialization_of_typed_results_is_only_called_once()
        {
            var called = 0;

            var result = await _g
                .ConfigureEnvironment(env => env
                    .ConfigureDeserializer(d => d
                        .Add(Create<JValue, BinaryData>((jValue, _, _, _) =>
                        {
                            if (jValue.Value is 42L)
                            {
                                Interlocked.Increment(ref called);

                                return new BinaryData(new byte[] { 42 });
                            }

                            return null;
                        }))))
                .Inject(42)
                .Cast<BinaryData>()
                .FirstAsync();

            result?
                .ToArray()
                .Should()
                .Contain(42);

            Interlocked
                .CompareExchange(ref called, 0, 1)
                .Should()
                .Be(1);
        }
    }
}
