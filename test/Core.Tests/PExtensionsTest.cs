using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class PExtensionsTest
    {
        private sealed class WithoutPFactory : IPFactory
        {
            public P? TryGetP(ExpressionSemantics semantics, object? maybeValue, IGremlinQueryEnvironment environment) => semantics is EqualsExpressionSemantics
                ? P.Without(1, 2)
                : null;
        }

        [Fact]
        public void Where_keeps_a_without_predicate_with_values() => g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                .ConfigureOptions(options => options
                    .ConfigureValue(PFactory.PFactoryOption, factory => factory.Override(new WithoutPFactory()))))
            .V<RichVertex>()
            .Values(x => x.Age)
            .Where(x => x == 5)
            .ToTraversal()
            .Steps[^1]
            .Should()
            .BeOfType<IsStep>();

        [Fact]
        public void Within_no_values_equals_false() => P.Within()
            .EqualsConstant(false)
            .Should()
            .BeTrue();

        [Fact]
        public void Without_no_values_equals_true() => P.Without()
            .EqualsConstant(true)
            .Should()
            .BeTrue();

        [Fact]
        public void Without_values_is_not_constant() => P.Without(1, 2)
            .EqualsConstant(true)
            .Should()
            .BeFalse();

        [Fact]
        public void Without_a_single_value_is_not_constant() => new P("without", 1)
            .EqualsConstant(true)
            .Should()
            .BeFalse();

        [Fact]
        public void And_of_two_true_predicates_equals_true() => P.Without()
            .And(P.Without())
            .EqualsConstant(true)
            .Should()
            .BeTrue();

        [Fact]
        public void And_with_a_false_predicate_equals_false() => P.Gt(1)
            .And(P.Within())
            .EqualsConstant(false)
            .Should()
            .BeTrue();

        [Fact]
        public void Or_with_a_true_predicate_equals_true() => P.Gt(1)
            .Or(P.Without())
            .EqualsConstant(true)
            .Should()
            .BeTrue();

        [Fact]
        public void Or_of_two_false_predicates_equals_false() => P.Within()
            .Or(P.Within())
            .EqualsConstant(false)
            .Should()
            .BeTrue();

        [Fact]
        public void True_equals_true() => new P("true", null)
            .EqualsConstant(true)
            .Should()
            .BeTrue();

        [Fact]
        public void False_equals_false() => new P("false", null)
            .EqualsConstant(false)
            .Should()
            .BeTrue();

        [Fact]
        public void StartingWith_an_empty_string_becomes_true() => TextP.StartingWith("")
            .WorkaroundLimitations(GremlinQueryEnvironment.Invalid)
            .OperatorName
            .Should()
            .Be("true");

        [Fact]
        public void StartingWith_a_string_ending_in_char_MaxValue_is_bounded_by_appending_char_MinValue() => TextP.StartingWith($"a{char.MaxValue}")
            .WorkaroundLimitations(GremlinQueryEnvironment.Invalid
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
            .Should()
            .BeEquivalentTo(P.Between($"a{char.MaxValue}", $"a{char.MaxValue}{char.MinValue}"));
    }
}
