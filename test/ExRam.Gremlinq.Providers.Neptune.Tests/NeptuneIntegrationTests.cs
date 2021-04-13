#if RELEASE && NET5_0 && RUNNEPTUNEINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : QueryIntegrationTest, IClassFixture<NeptuneFixture>
    {
        private static readonly Regex IdRegex1 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);
        private static readonly Regex IdRegex2 = new("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:Int32\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        public NeptuneIntegrationTests(NeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex1.Replace(x, "\"scrubbed id\""))
                .Add(x => IdRegex2.Replace(x, "\"scrubbed id\""));
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
    }

}
#endif
