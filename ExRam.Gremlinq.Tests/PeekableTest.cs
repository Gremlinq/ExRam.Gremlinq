using System.Linq;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class PeekableTest
    {
        [Fact]
        public void Test1()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);
                e.Current.Should().Be(1);

                var p = e.WithPebbles();
                p.DropPebble();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.Return();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(3);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(4);

                p.MoveNext().Should().Be(false);
            }
        }

        [Fact]
        public void Test2()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);
                e.Current.Should().Be(1);

                var p = e.WithPebbles();
                p.DropPebble();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.Return();

                p.Current.Should().Be(1);

                p = p.WithPebbles();
                p.DropPebble();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.Return();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(3);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(4);

                p.MoveNext().Should().Be(false);
            }
        }

        [Fact]
        public void Test3()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);

                var p = e.WithPebbles();
                p.DropPebble();

                p.Current.Should().Be(1);

                p.Return();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.Return();
                p.Current.Should().Be(1);

                p = p.WithPebbles();
                p.DropPebble();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.Return();
                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(3);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(4);

                p.MoveNext().Should().Be(false);
            }
        }

        [Fact]
        public void Test4()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);

                using (var p1 = e.WithPebbles())
                {
                    p1.DropPebble();

                    p1.Current.Should().Be(1);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(2);

                    p1.Return();

                    using (var p2 = p1.WithPebbles())
                    {
                        p2.DropPebble();

                        p2.Current.Should().Be(1);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(2);

                        p2.Return();
                        p2.Current.Should().Be(1);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(2);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(3);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(4);

                        p2.MoveNext().Should().Be(false);
                    }
                }
            }
        }

        [Fact]
        public void Test5()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);

                var p = e.WithPebbles();
                p.DropPebble();

                p.MoveNext().Should().Be(true);
                p.MoveNext().Should().Be(true);
                p.MoveNext().Should().Be(true);
                p.MoveNext().Should().Be(false);

                p.Return();

                p.Current.Should().Be(1);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(2);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(3);

                p.MoveNext().Should().Be(true);
                p.Current.Should().Be(4);

                p.MoveNext().Should().Be(false);
            }
        }

        [Fact]
        public void Test6()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);

                using (var p1 = e.WithPebbles())
                {
                    p1.DropPebble();

                    p1.Current.Should().Be(1);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(2);

                    using (var p2 = p1.WithPebbles())
                    {
                        p2.DropPebble();

                        p2.Current.Should().Be(2);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(3);

                        p2.Return();
                        p2.Current.Should().Be(2);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(3);

                        p2.MoveNext().Should().Be(true);
                        p2.Current.Should().Be(4);

                        p2.MoveNext().Should().Be(false);
                    }
                }
            }
        }

        [Fact]
        public void Test7()
        {
            using (var e = new[] { 1, 2, 3, 4 }
                .AsEnumerable()
                .GetEnumerator())
            {
                e.MoveNext().Should().Be(true);

                using (var p1 = e.WithPebbles())
                {
                    p1.DropPebble();

                    p1.Current.Should().Be(1);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(2);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(3);

                    p1.Return();

                    p1.Current.Should().Be(1);

                    p1.LiftPebble();
                    p1.DropPebble();

                    p1.Current.Should().Be(1);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(2);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(3);

                    p1.MoveNext().Should().Be(true);
                    p1.Current.Should().Be(4);

                    p1.Return();

                    p1.Current.Should().Be(1);
                }
            }
        }
    }
}
