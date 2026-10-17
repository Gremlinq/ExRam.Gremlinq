using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinqTestRunnerTest
    {
        [Fact]
        public async Task A_SettingsTask_becomes_the_task_of_its_verification()
        {
            var result = GremlinqTestRunner.ConvertTestMethodResult(Verifier.Verify("verified through the converted task"));

            await result
                .Should()
                .BeAssignableTo<Task>()
                .Subject;
        }

        [Fact]
        public async Task A_SettingsTask_without_snapshot_becomes_a_faulted_task()
        {
            var directory = Directory.CreateTempSubdirectory().FullName;

            try
            {
                var result = GremlinqTestRunner.ConvertTestMethodResult(Verifier
                    .Verify("no snapshot holds this")
                    .UseDirectory(directory)
                    .UseFileName("Missing")
                    .DisableDiff());

                // Verify's VerifyException is internal, so the message tells a missing snapshot from any other error.
                await result
                    .Should()
                    .BeAssignableTo<Task>()
                    .Subject
                    .Awaiting(task => task)
                    .Should()
                    .ThrowAsync<Exception>()
                    .WithMessage("*New:*");
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [Fact]
        public void A_Task_passes_through()
        {
            var task = Task.FromResult(42);

            GremlinqTestRunner
                .ConvertTestMethodResult(task)
                .Should()
                .BeSameAs(task);
        }

        [Fact]
        public void A_ValueTask_passes_through()
        {
            var valueTask = new ValueTask(Task.FromResult(42));

            GremlinqTestRunner
                .ConvertTestMethodResult(valueTask)
                .Should()
                .Be(valueTask);
        }

        [Fact]
        public void Null_passes_through()
        {
            GremlinqTestRunner
                .ConvertTestMethodResult(null)
                .Should()
                .BeNull();
        }

        [Fact]
        public void Another_result_throws()
        {
            FluentActions
                .Invoking(() => GremlinqTestRunner.ConvertTestMethodResult(42))
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*System.Int32*");
        }

        // Fails with Verify's "New:" message if the snapshot is missing, so it shows that the installed runner awaits it.
        [Fact]
        public SettingsTask A_fact_can_return_the_SettingsTask_of_its_verification() => Verifier.Verify("verified through the installed runner");
    }
}
