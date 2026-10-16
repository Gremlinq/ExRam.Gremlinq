using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class CanonicalComparerTests
    {
        private const string Ordered = """
            [
              1,
              2,
              3
            ]
            """;

        private static readonly IReadOnlyDictionary<string, object> Context = new Dictionary<string, object>();

        [Fact]
        public async Task Reordered_items_are_equal()
        {
            const string reordered = """
                [
                  3,
                  1,
                  2
                ]
                """;

            var result = await SnapshotComparer.Canonical(reordered, Ordered, Context);

            result.IsEqual.Should().BeTrue();
        }

        [Fact]
        public async Task Another_multiplicity_is_not_equal_and_the_message_names_the_first_differing_line()
        {
            const string twice = """
                [
                  7,
                  7
                ]
                """;

            const string once = """
                [
                  7
                ]
                """;

            var result = await SnapshotComparer.Canonical(twice, once, Context);

            result.IsEqual.Should().BeFalse();
            result.Message.Should().Be("The canonical forms differ first at line 3.\nReceived:   7\nVerified: ]");
        }

        [Fact]
        public async Task Two_exception_documents_with_different_messages_are_equal()
        {
            const string first = """
                {
                  Type: GremlinQueryExecutionException,
                  Message: Executing query 12345678-9012-3456-7890-123456789012 failed.,
                  InnerException: {
                    Type: ResponseException,
                    Message: ServerErrorFailStep: There's been an error.
                  }
                }
                """;

            const string second = """
                {
                  Type: GremlinQueryExecutionException,
                  Message: Something else went wrong.,
                  InnerException: {
                    Type: InvalidOperationException,
                    Message: Of its own.
                  }
                }
                """;

            var result = await SnapshotComparer.Canonical(first, second, Context);

            result.IsEqual.Should().BeTrue();
        }

        [Fact]
        public Task Reordered_items_verify_canonically() => Verifier
            .Verify(new[] { 3, 1, 2 })
            .UseCanonicalComparer();

        [Fact]
        public async Task UseDefaultComparer_after_UseCanonicalComparer_compares_exactly()
        {
            var directory = Directory.CreateTempSubdirectory().FullName;

            try
            {
                await File.WriteAllTextAsync(Path.Combine(directory, "Ordered.verified.txt"), Ordered, TestContext.Current.CancellationToken);

                var verify = () => Verifier
                    .Verify(new[] { 3, 1, 2 })
                    .UseDirectory(directory)
                    .UseFileName("Ordered")
                    .DisableDiff()
                    .UseCanonicalComparer()
                    .UseDefaultComparer()
                    .ToTask();

                // Verify's VerifyException is internal, so the message tells a failed comparison from any other error.
                await verify
                    .Should()
                    .ThrowAsync<Exception>()
                    .WithMessage("*NotEqual:*Verified: Ordered.verified.txt*");
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
