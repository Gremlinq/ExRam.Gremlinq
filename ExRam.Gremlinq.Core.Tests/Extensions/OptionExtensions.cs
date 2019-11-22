using System;
using FluentAssertions;
using FluentAssertions.Execution;

namespace LanguageExt
{
    public static class OptionExtensions
    {
        #region OptionAssertions<T>
        public sealed class OptionAssertions<T>
        {
            public OptionAssertions(Option<T> option)
            {
                Subject = option;
            }

            public Option<T> Subject { get; }

            public AndConstraint<OptionAssertions<T>> BeEqual(Option<T> other, string because = "", params object[] becauseArgs)
            {
                return Subject.Match(
                    some => other.Should().BeSome(some),
                    () => other.Should().BeNone());
            }

            public AndConstraint<OptionAssertions<T>> BeSome(T expected)
            {
                return BeSome(value => value.Should().Be(expected));
            }

            public AndConstraint<OptionAssertions<T>> BeSome(Action<T> someAction, string because = "", params object[] becauseArgs)
            {
                Execute
                    .Assertion
                    .ForCondition(Subject.IsSome)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected Some, but found None.");

                someAction(Subject.IfNoneUnsafe(default(T)));

                return new AndConstraint<OptionAssertions<T>>(this);
            }

            public AndConstraint<OptionAssertions<T>> BeNone(string because = "", params object[] becauseArgs)
            {
                Execute
                    .Assertion
                    .ForCondition(Subject.IsNone)
                    .BecauseOf(because, becauseArgs)
                    .FailWith($"Expected None, but found {Subject}.");

                return new AndConstraint<OptionAssertions<T>>(this);
            }
        }
        #endregion

        public static OptionAssertions<T> Should<T>(this Option<T> option)
        {
            return new OptionAssertions<T>(option);
        }
    }
}
