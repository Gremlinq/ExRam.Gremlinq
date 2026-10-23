using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/>, serialized for Neptune engine 1.5.0.0 and verified against the answers of
    /// <see cref="SerializationTests"/>. That engine understands TinkerPop 3.8.0 to 3.8.1 only. A fact whose query TinkerPop 3.8 needs
    /// in another form is overridden with its own answer.
    /// </summary>
    public class SerializationTestsEngine15 : QueryExecutionTest, IClassFixture<NeptuneEngine15ContainerFixture>, ISourceFileNameProvider<SerializationTestsEngine15>
    {
        public new class Verifier : SerializingVerifier<Bytecode>
        {
            public Verifier() : base()
            {

            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseSnapshotDirectoryAndNameOf<SerializationTests>();
        }

        public SerializationTestsEngine15(NeptuneEngine15ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // 3.8 renamed none() to discard():
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#none-and-discard
        public override SettingsTask And_none() => base.And_none().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask And_none_with_sideEffect() => base.And_none_with_sideEffect().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask None() => base.None().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask None_None() => base.None_None().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Or_none_with_sideEffect() => base.Or_none_with_sideEffect().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Project2_guarded() => base.Project2_guarded().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Project3_Select2_unguarded() => base.Project3_Select2_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Project3_unguarded() => base.Project3_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Project4_unguarded() => base.Project4_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_constant_bool_false() => base.Where_constant_bool_false().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_constant_bool_field_false() => base.Where_constant_bool_field_false().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_empty_array_intersects_property_array() => base.Where_empty_array_intersects_property_array().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_none_traversal() => base.Where_none_traversal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_or_dead_traversal() => base.Where_or_dead_traversal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_or_none_traversal() => base.Where_or_none_traversal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_array_intersects_empty_array() => base.Where_property_array_intersects_empty_array().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_compared_to_string_always_false() => base.Where_property_compared_to_string_always_false().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_comparison_to_string_always_false() => base.Where_property_comparison_to_string_always_false().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_comparison_to_string_always_false_2() => base.Where_property_comparison_to_string_always_false_2().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_comparison_to_string_always_false_3() => base.Where_property_comparison_to_string_always_false_3().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_comparison_to_string_always_false_4() => base.Where_property_comparison_to_string_always_false_4().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_is_contained_in_empty_enumerable() => base.Where_property_is_contained_in_empty_enumerable().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_variable_bool_property_false() => base.Where_variable_bool_property_false().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        // 3.8 removed has(key, traversal). The server still accepts it, but the traversal never matches:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#removal-of-has-key-traversal
        public override SettingsTask StepLabel_of_array_contains_element() => base.StepLabel_of_array_contains_element().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask StepLabel_of_array_contains_element_graphson() => base.StepLabel_of_array_contains_element_graphson().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask StepLabel_of_object_array_contains_element() => base.StepLabel_of_object_array_contains_element().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_array_contains_stepLabel() => base.Where_property_array_contains_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_array_intersects_stepLabel1() => base.Where_property_array_intersects_stepLabel1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_array_intersects_stepLabel2() => base.Where_property_array_intersects_stepLabel2().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_equals_stepLabel() => base.Where_property_equals_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_is_greater_than_or_equal_stepLabel() => base.Where_property_is_greater_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_is_greater_than_stepLabel() => base.Where_property_is_greater_than_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_is_lower_than_or_equal_stepLabel() => base.Where_property_is_lower_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_is_lower_than_stepLabel() => base.Where_property_is_lower_than_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Where_property_traversal() => base.Where_property_traversal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        // 3.8 removed aggregate(local, x), the server knows no such method. local(aggregate(x)) takes its place:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#removal-of-aggregate-with-scope-and-store
        public override SettingsTask Aggregate_Local() => base.Aggregate_Local().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Aggregate_Local_with_existing_step() => base.Aggregate_Local_with_existing_step().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        // 3.8's choose() passes unmatched and unproductive traversers through, where 3.7 filters the unmatched ones and fails
        // on the unproductive ones. option(none, discard()) without a default filters the unmatched ones and
        // option(unproductive, fail(…)) fails on the unproductive ones:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#choose-semantics
        public override SettingsTask Choose_one_case() => base.Choose_one_case().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Choose_only_default_case() => base.Choose_only_default_case().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Choose_two_cases() => base.Choose_two_cases().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Choose_two_cases_default() => base.Choose_two_cases_default().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        // 3.8's limit(local, 1), tail(local, 1) and range(local, n, n + 1) return their single result in a list, as the workaround
        // map(unfold().limit(1).fold()) and its like do on 3.7. The plain step takes the workaround's place:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#consistent-output-for-range-limit-tail
        public override SettingsTask Inject_Limit0_Fold_LimitLocal_1() => base.Inject_Limit0_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit0_Fold_RangeLocal_1() => base.Inject_Limit0_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit0_Fold_TailLocal_1() => base.Inject_Limit0_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit1_Fold_LimitLocal_1() => base.Inject_Limit1_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit1_Fold_RangeLocal_1() => base.Inject_Limit1_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit1_Fold_TailLocal_1() => base.Inject_Limit1_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit2_Fold_LimitLocal_1() => base.Inject_Limit2_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit2_Fold_RangeLocal_1() => base.Inject_Limit2_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask Inject_Limit2_Fold_TailLocal_1() => base.Inject_Limit2_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask LimitLocal() => base.LimitLocal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();

        public override SettingsTask TailLocal() => base.TailLocal().UseSnapshotDirectoryAndNameOf<SerializationTestsEngine15>();
    }
}
