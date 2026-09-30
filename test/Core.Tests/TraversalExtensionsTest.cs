using ExRam.Gremlinq.Core.Steps;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TraversalExtensionsTest
    {
        private static readonly Traversal Identity = IdentityStep.Instance;

        // A traversal that is not local, for steps that are local whatever is inside them.
        private static readonly Traversal GlobalLimit = Traversal.Empty.Push(OutStep.NoLabels, LimitStep.LimitGlobal1);

        // Steps that are local by themselves.
        private static readonly Step[] LocalSteps =
        [
            IdentityStep.Instance,

            new AndStep([GlobalLimit]),
            new CoinStep(0.5),
            new FilterStep.ByTraversalStep(GlobalLimit),
            new HasKeyStep("key"),
            new HasLabelStep(["label"]),
            new HasNotStep("key"),
            new HasPredicateStep("key", P.Eq(1)),
            new HasStep("key"),
            new HasTraversalStep("key", GlobalLimit),
            new HasValueStep("value"),
            new IsStep(P.Eq(1)),
            NoneStep.Instance,
            new NotStep(GlobalLimit),
            new OrStep([GlobalLimit]),
            new WherePredicateStep(P.Eq("label")),
            new WhereStepLabelAndPredicateStep("label1", P.Eq("label2")),
            new WhereTraversalStep(GlobalLimit),

            OutStep.NoLabels,
            InStep.NoLabels,
            BothStep.NoLabels,
            OutEStep.NoLabels,
            InEStep.NoLabels,
            BothEStep.NoLabels,
            OutVStep.Instance,
            InVStep.Instance,
            BothVStep.Instance,
            OtherVStep.Instance,
            PropertiesStep.All,
            ValuesStep.All,
            UnfoldStep.Instance,

            IdStep.Instance,
            LabelStep.Instance,
            KeyStep.Instance,
            ValueStep.Instance,
            ValueMapStep.All,
            ElementMapStep.Instance,
            new ConstantStep(1),
            new SelectColumnStep(Column.Keys),
            new SelectKeysStep("key"),

            AsStringStep.Instance,
            AsDateStep.Instance,
            new DateAddStep(DT.Day, 1),
            new DateDiffStep.Constant(DateTimeOffset.UnixEpoch),
            new DateDiffStep.Traversal(GlobalLimit),
            new ConcatStringsStep(["string"]),
            new ConcatTraversalsStep([GlobalLimit]),
            new FormatStep("format", []),
            LengthStep.Global,
            new ReplaceStep("old", "new"),
            ReverseStep.Instance,
            new SubstringStep(1..2, Scope.Global),
            ToLowerStep.Global,
            ToUpperStep.Global,
            TrimStep.Global,
            TrimStartStep.Global,
            TrimEndStep.Global,

            new MapStep(GlobalLimit),
            new FlatMapStep(GlobalLimit),
            new LocalStep(GlobalLimit),
            new CoalesceStep([GlobalLimit, Identity]),
            new ProjectStep(["name"]),

            LimitStep.LimitLocal1,
            new RangeStep(0, 1, Scope.Local),
            TailStep.TailLocal1,
            new SkipStep(1, Scope.Local),
            DedupStep.Local,
            CountStep.Local,
            OrderStep.Local,
            MinStep.Local,
            MaxStep.Local,
            MeanStep.Local,
            SumStep.Local
        ];

        // Modulators, each with a step it belongs to. They are local after that step, but not at the start of a traversal.
        private static readonly (Step Parent, Step Modulator)[] ModulatorSteps =
        [
            (new ProjectStep(["name"]), new ProjectStep.ByTraversalStep(GlobalLimit)),
            (new ProjectStep(["name"]), new ProjectStep.ByKeyStep("key")),
            (new FormatStep("format", []), new FormatStep.By(GlobalLimit)),
            (new WherePredicateStep(P.Eq("label")), new WherePredicateStep.ByMemberStep("key")),
            (OrderStep.Local, new OrderStep.ByMemberStep("key", Order.Asc)),
            (OrderStep.Local, new OrderStep.ByTraversalStep(GlobalLimit, Order.Asc))
        ];

        // Steps that are not local.
        private static readonly Step[] OtherSteps =
        [
            LimitStep.LimitGlobal1,
            new RangeStep(0, 1, Scope.Global),
            TailStep.TailGlobal1,
            new SkipStep(1, Scope.Global),
            DedupStep.Global,
            CountStep.Global,
            OrderStep.Global,
            MinStep.Global,
            MaxStep.Global,
            MeanStep.Global,
            SumStep.Global,

            new AsStep("label"),
            new SelectStepLabelStep("label"),

            FoldStep.Instance,
            GroupStep.Instance,
            new GroupStep.ByTraversalStep(Identity),
            new GroupStep.ByKeyStep("key"),
            TreeStep.Instance,
            TreeStep.ByIdentityStep.Instance,
            new TreeStep.ByKeyStep("key"),
            BarrierStep.Instance,
            new CapStep("label"),
            new AggregateStep(Scope.Local, "label"),

            new UnionStep([Identity]),
            new ChooseOptionTraversalStep(Identity),
            new ChoosePredicateStep(P.Eq(1), Identity),
            new ChooseTraversalStep(Identity, Identity),
            new OptionTraversalStep(null, Identity),
            new OptionalStep(Identity),
            new RepeatStep(Identity),
            new UntilStep(Identity),
            EmitStep.Instance,
            new TimesStep(1),
            new SideEffectStep(Identity),
            new MatchStep([Identity]),

            VStep.NoIds,
            EStep.NoIds,
            new InjectStep([1]),
            new WithSideEffectStep("label", 1),

            PathStep.Instance,
            SimplePathStep.Instance,
            CyclicPathStep.Instance,
            ProfileStep.Instance,
            ExplainStep.Instance,
            FailStep.NoMessage,

            new AddVStep("label"),
            new AddEStep("label"),
            new AddEStep.FromLabelStep("label"),
            new AddEStep.FromTraversalStep(Identity),
            new AddEStep.ToLabelStep("label"),
            new AddEStep.ToTraversalStep(Identity),
            new PropertyStep.ByKeyStep("key", 1),
            new PropertyStep.ByTraversalStep(Identity, 1),
            DropStep.Instance
        ];

        [Fact]
        public void IsLocal_empty() => Traversal.Empty
            .IsLocal()
            .Should()
            .BeTrue();

        [Fact]
        public void IsLocal_local_steps() => LocalSteps
            .Where(static step => !Traversal.Empty.Push(step).IsLocal())
            .Select(static step => step.GetType())
            .Should()
            .BeEmpty();

        [Fact]
        public void IsLocal_other_steps() => OtherSteps
            .Where(static step => Traversal.Empty.Push(step).IsLocal())
            .Select(static step => step.GetType())
            .Should()
            .BeEmpty();

        [Fact]
        public void IsLocal_modulator_after_its_step() => ModulatorSteps
            .Where(static steps => !Traversal.Empty.Push(steps.Parent, steps.Modulator).IsLocal())
            .Select(static steps => steps.Modulator.GetType())
            .Should()
            .BeEmpty();

        [Fact]
        public void IsLocal_modulator_as_first_step() => ModulatorSteps
            .Where(static steps => Traversal.Empty.Push(steps.Modulator).IsLocal())
            .Select(static steps => steps.Modulator.GetType())
            .Should()
            .BeEmpty();

        [Fact]
        public void IsLocal_modulator_after_global_step() => Traversal.Empty
            .Push(OrderStep.Global, new OrderStep.ByMemberStep("key", Order.Asc))
            .IsLocal()
            .Should()
            .BeFalse();

        [Fact]
        public void IsLocal_many_local_steps() => Traversal.Empty
            .Push(new HasLabelStep(["label"]), OutStep.NoLabels, new ValuesStep(["key"]), LimitStep.LimitLocal1)
            .IsLocal()
            .Should()
            .BeTrue();

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void IsLocal_one_other_step(int index)
        {
            var steps = new Step[] { new HasLabelStep(["label"]), OutStep.NoLabels, new ValuesStep(["key"]) };

            steps[index] = LimitStep.LimitGlobal1;

            Traversal.Empty
                .Push(steps)
                .IsLocal()
                .Should()
                .BeFalse();
        }

        [Fact]
        public void IsLocal_write_step_within_local_step() => Traversal.Empty
            .Push(new MapStep(new AddVStep("label")))
            .IsLocal()
            .Should()
            .BeFalse();

        // Fails for a step that none of the lists above mentions: whether it is local has to be decided.
        [Fact]
        public void IsLocal_is_decided_for_every_step() => typeof(Step).Assembly
            .GetTypes()
            .Where(static type => !type.IsAbstract && typeof(Step).IsAssignableFrom(type))
            .Except(LocalSteps
                .Concat(OtherSteps)
                .Concat(ModulatorSteps.Select(static steps => steps.Modulator))
                .Select(static step => step.GetType()))
            .Should()
            .BeEmpty();
    }
}
