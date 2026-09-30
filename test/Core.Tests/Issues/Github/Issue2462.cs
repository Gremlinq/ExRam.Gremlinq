using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue2462 : GremlinqTestBase
    {
        public Issue2462() : base(new DebugGremlinQueryVerifier())
        {

        }

        [Fact]
        public Task Repro() => Verify(
            new HasLabelStep(["A"]),
            new ProjectStep(["x"]),
            new ProjectStep.ByTraversalStep(IdentityStep.Instance),
            Coalesce(IdentityStep.Instance));

        [Fact]
        public Task Identity_child() => Verify(
            Coalesce(IdentityStep.Instance));

        [Fact]
        public Task Identities_child() => Verify(
            Coalesce(Traversal.Empty.Push(IdentityStep.Instance, IdentityStep.Instance)));

        [Fact]
        public Task Empty_child() => Verify(
            Coalesce(Traversal.Empty));

        [Fact]
        public Task Identity_child_between_steps() => Verify(
            OutStep.NoLabels,
            Coalesce(IdentityStep.Instance),
            InStep.NoLabels);

        [Fact]
        public Task HasLabel_child_after_HasLabel() => Verify(
            new HasLabelStep(["A", "B"]),
            Coalesce(new HasLabelStep(["B", "C"])));

        [Fact]
        public Task Has_child_after_Has() => Verify(
            new HasPredicateStep("Age", P.Gt(36)),
            Coalesce(new HasPredicateStep("Age", P.Lt(42))));

        [Fact]
        public Task Out_Values_child() => Verify(
            Coalesce(Traversal.Empty.Push(OutStep.NoLabels, new ValuesStep(["Name"]))));

        [Fact]
        public Task Limit_local_child() => Verify(
            FoldStep.Instance,
            Coalesce(LimitStep.LimitLocal1));

        [Fact]
        public Task Map_Out_Limit_child() => Verify(
            Coalesce(new MapStep(Traversal.Empty.Push(OutStep.NoLabels, LimitStep.LimitGlobal1))));

        [Fact]
        public Task Coalesce_Out_child() => Verify(
            Coalesce(Coalesce(OutStep.NoLabels)));

        [Fact]
        public Task Coalesce_Out_Limit_child() => Verify(
            Coalesce(Coalesce(Traversal.Empty.Push(OutStep.NoLabels, LimitStep.LimitGlobal1))));

        [Fact]
        public Task Coalesce_Out_In_child() => Verify(
            Coalesce(Coalesce(OutStep.NoLabels, InStep.NoLabels)));

        [Fact]
        public Task Identity_child_in_Map() => Verify(
            new MapStep(Coalesce(IdentityStep.Instance)));

        [Fact]
        public Task Coalesce_Identity_child_in_Map() => Verify(
            new MapStep(Coalesce(Coalesce(IdentityStep.Instance))));

        [Fact]
        public Task Identity_child_before_step_in_Map() => Verify(
            new MapStep(Traversal.Empty.Push(Coalesce(IdentityStep.Instance), OutStep.NoLabels)));

        [Fact]
        public Task Out_Limit_child() => Verify(
            Coalesce(Traversal.Empty.Push(OutStep.NoLabels, LimitStep.LimitGlobal1)));

        [Fact]
        public Task Out_Fold_child() => Verify(
            Coalesce(Traversal.Empty.Push(OutStep.NoLabels, FoldStep.Instance)));

        [Fact]
        public Task Count_child() => Verify(
            Coalesce(CountStep.Global));

        [Fact]
        public Task Out_As_child() => Verify(
            Coalesce(Traversal.Empty.Push(OutStep.NoLabels, new AsStep("a"))));

        [Fact]
        public Task AddV_child() => Verify(
            Coalesce(new AddVStep("Person")));

        [Fact]
        public Task Optional_Out_child() => Verify(
            Coalesce(new OptionalStep(OutStep.NoLabels)));

        [Fact]
        public Task By_child_after_Project() => Verify(
            new ProjectStep(["x"]),
            Coalesce(new ProjectStep.ByTraversalStep(IdentityStep.Instance)));

        [Fact]
        public Task Two_children() => Verify(
            Coalesce(OutStep.NoLabels, InStep.NoLabels));

        [Fact]
        public Task No_children() => Verify(
            Coalesce());

        private static CoalesceStep Coalesce(params ImmutableArray<Traversal> traversals) => new(traversals);

        private static Task Verify(params Step[] steps) => g
            .ConfigureEnvironment(env => env)
            .V()
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<object>>(
                traversal => traversal.Push(steps),
                static _ => Projection.Empty)
            .Verify();
    }
}
