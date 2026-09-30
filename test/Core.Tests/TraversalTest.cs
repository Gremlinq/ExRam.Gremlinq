using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TraversalTest
    {
        private readonly Traversal _traversal;
        private readonly IdentityStep _step1 = new ();
        private readonly IdentityStep _step2 = new ();
        private readonly IdentityStep _step3 = new ();
        private readonly IdentityStep _step4 = new ();
        private readonly IdentityStep _step5 = new ();
        private readonly IdentityStep _step6 = new ();

        public TraversalTest()
        {
            _traversal = Traversal.Empty
                .Push(_step1)
                .Push(_step2)
                .Push(_step3)
                .Push(_step4)
                .Push(_step5)
                .Push(_step6);
        }

        [Fact]
        public void Slice()
        {
            var sliced = _traversal.Slice(3, 2);

            sliced.Count.Should().Be(2);
            sliced[0].Should().Be(_step4);
            sliced[1].Should().Be(_step5);
        }

        [Fact]
        public void Slice_out_of_range()
        {
            var sliced = _traversal
                .Invoking(_ => _
                    .Slice(3, 5))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Slice_push()
        {
            var newStep = new IdentityStep();
            var sliced = _traversal.Slice(3, 2);

            sliced = sliced.Push(newStep);

            sliced[0].Should().Be(_step4);
            sliced[1].Should().Be(_step5);
            sliced[2].Should().Be(newStep);
        }

        [Fact]
        public void Slice_only_start()
        {
#pragma warning disable IDE0057 // Use range operator
            var sliced = _traversal.Slice(3);
#pragma warning restore IDE0057 // Use range operator

            sliced.Count.Should().Be(3);
            sliced[0].Should().Be(_step4);
            sliced[1].Should().Be(_step5);
            sliced[2].Should().Be(_step6);
        }

        [Fact]
        public void Slice_only_start_push()
        {
            var newStep = new IdentityStep();

#pragma warning disable IDE0057 // Use range operator
            var sliced = _traversal.Slice(3);
#pragma warning restore IDE0057 // Use range operator

            sliced = sliced.Push(newStep);

            sliced.Count.Should().Be(4);
            sliced[0].Should().Be(_step4);
            sliced[1].Should().Be(_step5);
            sliced[2].Should().Be(_step6);
            sliced[3].Should().Be(newStep);
        }

        [Theory]
        [InlineData("R")]
        [InlineData("W")]
        [InlineData("RRRRRR")]
        [InlineData("WRRRRR")]
        [InlineData("RRWRRR")]
        [InlineData("RRRRRW")]
        [InlineData("WRRRRW")]
        [InlineData("WRWRRW")]
        [InlineData("RWWRWR")]
        [InlineData("WWWWWW")]
        //More steps than the memory underneath a traversal initially holds.
        [InlineData("RWRRRRRRRRRRRRRRW")]
        [InlineData("RRRRRRRRRRRRRRRRR")]
        public void Slice_SideEffectSemantics(string steps)
        {
            var traversal = CreateTraversal(steps);

            traversal.Count.Should().Be(steps.Length);

            //Slices, and slices of these slices, the count of which is derived from a derived one.
            AssertSlices(traversal, 1);
        }

        [Theory]
        [InlineData("WRRRRR", SideEffectSemantics.Read)]
        [InlineData("RWRRRR", SideEffectSemantics.Write)]
        [InlineData("RRRRRW", SideEffectSemantics.Write)]
        [InlineData("WRRRRW", SideEffectSemantics.Write)]
        [InlineData("RRRRRR", SideEffectSemantics.Read)]
        public void Slice_without_first_step_SideEffectSemantics(string steps, SideEffectSemantics expected)
        {
            var sliced = CreateTraversal(steps)[1..];

            sliced.Count.Should().Be(steps.Length - 1);
            sliced.SideEffectSemantics.Should().Be(expected);

            AssertSideEffectSemantics(sliced);
        }

        [Theory]
        [InlineData("RRRRRW", SideEffectSemantics.Read)]
        [InlineData("RRRRWR", SideEffectSemantics.Write)]
        [InlineData("WRRRRR", SideEffectSemantics.Write)]
        [InlineData("WRRRRW", SideEffectSemantics.Write)]
        [InlineData("RRRRRR", SideEffectSemantics.Read)]
        public void Slice_without_last_step_SideEffectSemantics(string steps, SideEffectSemantics expected)
        {
            var sliced = CreateTraversal(steps)[..^1];

            sliced.Count.Should().Be(steps.Length - 1);
            sliced.SideEffectSemantics.Should().Be(expected);

            AssertSideEffectSemantics(sliced);
        }

        [Fact]
        public void Slice_of_read_only_traversal_SideEffectSemantics()
        {
            var sliced = _traversal.Slice(1, 4);

            sliced.SideEffectSemantics.Should().Be(SideEffectSemantics.Read);

            sliced = sliced.Push(DropStep.Instance);

            sliced.SideEffectSemantics.Should().Be(SideEffectSemantics.Write);

            AssertSideEffectSemantics(sliced);
        }

        [Fact]
        public void Slice_keeps_projection()
        {
            var sliced = CreateTraversal("RWRRWR")
                .WithProjection(Projection.Vertex)
                .Slice(1, 4);

            sliced.Projection.Should().BeSameAs(Projection.Vertex);
        }

        [Theory]
        [InlineData("RRRRRR")]
        [InlineData("WRRRRR")]
        [InlineData("RRWRRR")]
        [InlineData("RRRRRW")]
        [InlineData("WWWWWW")]
        public void Slice_arguments_out_of_range(string steps)
        {
            var traversal = CreateTraversal(steps);

            foreach (var (start, length) in new[] { (1, -1), (0, -1), (6, -1), (0, int.MinValue), (3, 5), (0, 7), (6, 1), (1, int.MaxValue), (-1, 0), (-1, 2), (int.MinValue, 0), (7, 0), (7, -1), (16, 0), (17, 0), (int.MaxValue, 0) })
            {
                traversal
                    .Invoking(_ => _
                        .Slice(start, length))
                    .Should()
                    .Throw<ArgumentOutOfRangeException>($"of a start of {start} and a length of {length}");
            }

            foreach (var start in new[] { -1, 7, 17, int.MinValue, int.MaxValue })
            {
                traversal
                    .Invoking(_ => _
#pragma warning disable IDE0057 // Use range operator
                        .Slice(start))
#pragma warning restore IDE0057 // Use range operator
                    .Should()
                    .Throw<ArgumentOutOfRangeException>($"of a start of {start}");
            }
        }

        //SideEffectSemantics only tells whether a traversal counts any write steps at all, so a count that is off
        //stays unnoticed as long as there are write steps left. Popping the traversal down step by step takes the
        //write steps away one by one, and a count that is too high or too low shows at the latest once none are left.
        private static void AssertSideEffectSemantics(Traversal traversal)
        {
            while (true)
            {
                traversal.SideEffectSemantics
                    .Should()
                    .Be(GetSideEffectSemantics(traversal.Steps));

                if (traversal.Count == 0)
                    break;

                traversal = traversal.Pop();
            }
        }

        private static void AssertSlices(Traversal traversal, int depth)
        {
            for (var start = 0; start <= traversal.Count; start++)
            {
                for (var length = 0; length <= traversal.Count - start; length++)
                {
                    var sliced = traversal.Slice(start, length);

                    sliced.Count.Should().Be(length);

                    sliced.Steps
                        .ToArray()
                        .Should()
                        .Equal(traversal.Steps[start..(start + length)].ToArray());

                    AssertSideEffectSemantics(sliced);

                    if (depth > 0)
                        AssertSlices(sliced, depth - 1);
                }
            }
        }

        private static Traversal CreateTraversal(string steps)
        {
            var traversal = Traversal.Empty;

            foreach (var step in steps)
            {
                traversal = traversal.Push(step switch
                {
                    'R' => new IdentityStep(),
                    'W' => new DropStep(),
                    _ => throw new ArgumentOutOfRangeException(nameof(steps))
                });
            }

            return traversal;
        }

        private static SideEffectSemantics GetSideEffectSemantics(ReadOnlySpan<Step> steps)
        {
            foreach (var step in steps)
            {
                if (step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write)
                    return SideEffectSemantics.Write;
            }

            return SideEffectSemantics.Read;
        }
    }
}
