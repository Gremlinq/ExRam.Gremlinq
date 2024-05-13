﻿using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Steps;
using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TraversalTest : VerifyBase
    {
        private readonly Traversal _traversal;
        private readonly IdentityStep _step1 = new ();
        private readonly IdentityStep _step2 = new ();
        private readonly IdentityStep _step3 = new ();
        private readonly IdentityStep _step4 = new ();
        private readonly IdentityStep _step5 = new ();
        private readonly IdentityStep _step6 = new ();

        public TraversalTest() : base()
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

        [Fact]
        public Task ToTraversal_extension()
        {
            return Verify(new Step []{ new InjectStep(ImmutableArray.Create<object>(1)), new InjectStep(ImmutableArray.Create<object>(2)) }.ToTraversal().Steps.ToArray());
        }

        [Fact]
        public Task ToTraversal_extension_2()
        {
            return Verify(new Step []{ new InjectStep(ImmutableArray.Create<object>(1)), new InjectStep(ImmutableArray.Create<object>(2)) }.AsEnumerable().ToTraversal().Steps.ToArray());
        }

        [Fact]
        public Task ToTraversal_extension_3()
        {
            return Verify(new Step []{ new InjectStep(ImmutableArray.Create<object>(1)), new InjectStep(ImmutableArray.Create<object>(2)) }.ToList().ToTraversal().Steps.ToArray());
        }
    }
}
