#if RELEASE && NET5_0 && RUNNEPTUNEINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : QueryIntegrationTest, IClassFixture<NeptuneIntegrationTests.Fixture>
    {
        public sealed class Fixture : IntegrationTestFixture
        {
            public Fixture() : base(Core.GremlinQuerySource.g
                .UseNeptune(builder => builder
                    .At(new Uri("ws://localhost:8182")))
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .TransformResult(_ => _.Where(x => false)))))
            {
            }
        }

        private static readonly Regex IdRegex1 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);
        
        public NeptuneIntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex1.Replace(x, "\"scrubbed id\""));
        }

        [Fact(Skip= "Properties on a vertex property is not supported")]
        public override Task AddV_with_Meta_with_properties() => base.AddV_with_Meta_with_properties();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task AddV_with_MetaModel() => base.AddV_with_MetaModel();

        [Fact(Skip = "Query parsing failed at line 1, character position at 0, error message : mismatched input '[' expecting {EmptyStringLiteral, 'g'}")]
        public override Task FilterWithLambda() => base.FilterWithLambda();

        [Fact(Skip = "Query parsing failed at line 1, character position at 0, error message : mismatched input '[' expecting {EmptyStringLiteral, 'g'}")]
        public override Task OrderBy_lambda() => base.OrderBy_lambda();

        [Fact(Skip = "Query parsing failed at line 1, character position at 0, error message : mismatched input '[' expecting {EmptyStringLiteral, 'g'}")]
        public override Task OrderBy_ThenBy_lambda() => base.OrderBy_ThenBy_lambda();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Meta_ValueMap() => base.Properties_Meta_ValueMap();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Meta_Values() => base.Properties_Meta_Values();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Meta_Values_Projected() => base.Properties_Meta_Values_Projected();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Meta_Where1() => base.Properties_Meta_Where1();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties_as_select() => base.Properties_Properties_as_select();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties_key() => base.Properties_Properties_key();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties_Value() => base.Properties_Properties_Value();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties_Where_key() => base.Properties_Properties_Where_key();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties1() => base.Properties_Properties1();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Properties2() => base.Properties_Properties2();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_ValueMap_typed() => base.Properties_ValueMap_typed();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_ValueMap_untyped() => base.Properties_ValueMap_untyped();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Values_typed() => base.Properties_Values_typed();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Values_untyped() => base.Properties_Values_untyped();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Properties_Values2() => base.Properties_Values2();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Property_single_with_dictionary_meta1() => base.Property_single_with_dictionary_meta1();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Property_single_with_meta() => base.Property_single_with_meta();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Set_Meta_Property1() => base.Set_Meta_Property1();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Set_Meta_Property2() => base.Set_Meta_Property2();

        [Fact(Skip = "Properties on a vertex property is not supported")]
        public override Task Variable_wrap() => base.Variable_wrap();

        [Fact(Skip = "An unexpected error has occurred in Neptune.")]
        public override Task Properties_Where_Dictionary_key1() => base.Properties_Where_Dictionary_key1();

        [Fact(Skip = "An unexpected error has occurred in Neptune.")]
        public override Task Properties_Where_Dictionary_key2() => base.Properties_Where_Dictionary_key2();

        [Fact(Skip = "An unexpected error has occurred in Neptune.")]
        public override Task Properties_Where_Meta_key() => base.Properties_Where_Meta_key();

        [Fact(Skip = "An unexpected error has occurred in Neptune.")]
        public override Task Properties_Where_Meta_key_reversed() => base.Properties_Where_Meta_key_reversed();

        [Fact(Skip = "An unexpected error has occurred in Neptune.")]
        public override Task Properties_Where_neq_Label() => base.Properties_Where_neq_Label();

        [Fact(Skip = "UnsupportedOperationException")]
        public override Task Set_Meta_Property_to_null() => base.Set_Meta_Property_to_null();
    }
}
#endif
