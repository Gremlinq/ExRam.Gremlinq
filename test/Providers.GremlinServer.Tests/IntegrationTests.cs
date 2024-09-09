using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;

using Newtonsoft.Json.Linq;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public IntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
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
        public Task StartsWith_case_insensitive() => _g
            .Inject("Hello", "Bello")
            .Where(x => x.StartsWith("he", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task Equals_case_insensitive() => _g
            .Inject("Hello", "Bello")
            .Where(x => x.Equals("hello", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task EndsWith_case_insensitive() => _g
            .Inject("Hello", "Hallx")
            .Where(x => x.EndsWith("lo", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task Contains_case_insensitive() => _g
            .Inject("Hello", "Snafu")
            .Where(x => x.Contains("LL", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task StartsWith_case_insensitive_reverse() => _g
            .Inject("Bello", "Hello")
            .Where(x => x.StartsWith("he", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task Equals_case_insensitive_reverse() => _g
            .Inject("Bello", "Hello")
            .Where(x => x.Equals("hello", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task EndsWith_case_insensitive_reverse() => _g
            .Inject("Hallx", "Hello")
            .Where(x => x.EndsWith("lo", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task Contains_case_insensitive_reverse() => _g
            .Inject("Snafu", "Hello")
            .Where(x => x.Contains("LL", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public Task DateTimeOffset_from_string_1() => _g
            .Inject("2016-12-14T16:39:19.349Z")
            .Cast<DateTimeOffset>()
            .Verify();

        [Fact(Skip = "Won't work on CI and everything that is not located in my time zone.")]
        public Task DateTimeOffset_from_string_2() => _g
            .Inject("2016-01-01T12:30")
            .Cast<DateTimeOffset>()
            .Verify();

        [Fact]
        public Task DateTimeOffset_from_string_3() => _g
            .Inject("2007-12-03T10:15:30+01:00")
            .Cast<DateTimeOffset>()
            .Verify();

        [Fact]
        public Task TimeSpan_from_int() => _g
            .Inject(42)
            .Cast<TimeSpan>()
            .Verify();

        [Fact]
        public Task TimeSpan_from_float() => _g
            .Inject(42.3)
            .Cast<TimeSpan>()
            .Verify();

        [Fact]
        public Task TimeSpan_from_malformed_string() => _g
            .Inject("abc")
            .Cast<TimeSpan>()
            .Awaiting(_ => _
                .FirstOrDefaultAsync())
            .Should()
            .ThrowAsync<GremlinQueryExecutionException>();

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

        [Fact]
        public Task Project_to_null_entity() => _g
            .V<Person>()
            .Limit(1)
            .Project(b => b
                .ToTuple()
                .By(__ => __
                    .Label())
                .By(__ => __
                    .None()))
            .Verify();
    }
}
