using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/>, serialized for TinkerPop 3.8.2 only and verified against the answers of
    /// <see cref="SerializationTests"/>. A fact whose query TinkerPop 3.8 needs in another form is overridden with its own answer.
    /// </summary>
    public class SerializationTests38 : QueryExecutionTest, IClassFixture<GremlinServer38ContainerFixture>, ISourceFileNameProvider<SerializationTests38>
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

        public SerializationTests38(GremlinServer38ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // 3.8 renamed none() to discard():
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#none-and-discard
        public override SettingsTask And_none() => base.And_none().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask And_none_with_sideEffect() => base.And_none_with_sideEffect().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask None() => base.None().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask None_None() => base.None_None().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Or_none_with_sideEffect() => base.Or_none_with_sideEffect().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Project2_guarded() => base.Project2_guarded().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Project3_Select2_unguarded() => base.Project3_Select2_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Project3_unguarded() => base.Project3_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Project4_unguarded() => base.Project4_unguarded().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_constant_bool_false() => base.Where_constant_bool_false().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_constant_bool_field_false() => base.Where_constant_bool_field_false().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_empty_array_intersects_property_array() => base.Where_empty_array_intersects_property_array().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_none_traversal() => base.Where_none_traversal().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_or_dead_traversal() => base.Where_or_dead_traversal().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_or_none_traversal() => base.Where_or_none_traversal().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_array_intersects_empty_array() => base.Where_property_array_intersects_empty_array().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_compared_to_string_always_false() => base.Where_property_compared_to_string_always_false().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_comparison_to_string_always_false() => base.Where_property_comparison_to_string_always_false().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_comparison_to_string_always_false_2() => base.Where_property_comparison_to_string_always_false_2().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_comparison_to_string_always_false_3() => base.Where_property_comparison_to_string_always_false_3().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_comparison_to_string_always_false_4() => base.Where_property_comparison_to_string_always_false_4().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_is_contained_in_empty_enumerable() => base.Where_property_is_contained_in_empty_enumerable().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_variable_bool_property_false() => base.Where_variable_bool_property_false().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        // 3.8 removed has(key, traversal). The server still accepts it, but the traversal never matches:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#removal-of-has-key-traversal
        public override SettingsTask StepLabel_of_array_contains_element() => base.StepLabel_of_array_contains_element().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask StepLabel_of_array_contains_element_graphson() => base.StepLabel_of_array_contains_element_graphson().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask StepLabel_of_object_array_contains_element() => base.StepLabel_of_object_array_contains_element().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_array_contains_stepLabel() => base.Where_property_array_contains_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_array_intersects_stepLabel1() => base.Where_property_array_intersects_stepLabel1().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_array_intersects_stepLabel2() => base.Where_property_array_intersects_stepLabel2().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_equals_stepLabel() => base.Where_property_equals_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_is_greater_than_or_equal_stepLabel() => base.Where_property_is_greater_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_is_greater_than_stepLabel() => base.Where_property_is_greater_than_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_is_lower_than_or_equal_stepLabel() => base.Where_property_is_lower_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_is_lower_than_stepLabel() => base.Where_property_is_lower_than_stepLabel().UseSnapshotDirectoryAndNameOf<SerializationTests38>();

        public override SettingsTask Where_property_traversal() => base.Where_property_traversal().UseSnapshotDirectoryAndNameOf<SerializationTests38>();
    }
}
