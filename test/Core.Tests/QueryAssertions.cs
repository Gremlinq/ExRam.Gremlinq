using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class QueryAssertions
    {
        private readonly IGremlinQuerySource _g;

        public QueryAssertions()
        {
            _g = g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));
        }

        [ReleaseOnlyFact]
        public virtual void VertexProperty_throws_on_null_value()
        {
            default(int)
                .Invoking(_ => new VertexProperty<string>(null!))
                .Should()
                .Throw<ArgumentNullException>();

            new VertexProperty<string>("")
                .Invoking(_ => _.Value = null!)
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public virtual async Task Aggregate_Cap_type()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .Should()
                .BeAssignableTo<IGremlinQueryBase<Person[]>>();
        }

        [Fact]
        public virtual void And_without_parameters()
        {
            _g
                .V<Person>()
                .Invoking(__ => __
                    .And()
                    .Out())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual void Cast_to_same_type_yields_same_query()
        {
            var original = _g
                .V<Person>();

            var cast = original
                .Cast<Person>();

            original
                .Should()
                .BeSameAs(cast);
        }

        [Fact]
        public virtual async Task Coalesce_empty()
        {
            _g
                .V()
                .Invoking(__ => __.Coalesce<IGremlinQueryBase>())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task Limit_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Limit(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual void Mid_query_g_throws()
        {
            _g
                .V()
                .Invoking(_ => _
                    .Coalesce(
                        __ => _g.V<object>(),
                        __ => __.AddV<object>()))
                .Should()
                .Throw<InvalidOperationException>();
        }


        [ReleaseOnlyFact]
        public virtual void NullGuard_works()
        {
            _g
                .Invoking(_ => _
                    .V<Company>(default!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public virtual void Or_without_parameters()
        {
            _g
                .V<Person>()
                .Out()
                .Invoking(__ => __
                    .Or()
                    .In())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual void Property_single_with_dictionary_meta2()
        {
            _g
                .V<Country>()
                .Invoking(_ => _
                    .Property(x => x.LocalizableDescription, new VertexProperty<string, IDictionary<string, string>>("")
                    {
                        Properties = new Dictionary<string, string>
                        {
                            { "someKey", "value" }
                        }
                    }))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public virtual void Range_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Range(-1, 0))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task Select_with_unknown_label_throws()
        {
            _g
                .Inject(0)
                .Invoking(_ => _
                    .Select<int>("label"))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public virtual void Skip_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Skip(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual void Tail_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Tail(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual void Vertex_comparison_with_null_throws()
        {
            _g
                .V<Person>()
                .Invoking(x => x
                    .Where(y => y != null)
                    .Debug())
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public virtual async Task Where_native_type_property_length()
        {
            _g
                .V<Person>()
                .Invoking(_ => _.Where(t => t.Image!.Length == 3))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public virtual void Where_property_contains_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.Containing)))
                .V<Country>()
                .Invoking(_ =>
                    _.Where(c => c.CountryCallingCode!.Contains("456")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Invoking(_ => _
                    .Where(c => c.CountryCallingCode!.EndsWith("7890")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }
    }
}
