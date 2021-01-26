#pragma warning disable 660,661
#pragma warning disable IDE0060
using System;

namespace ExRam.Gremlinq.Core
{
    public abstract class StepLabel
    {
        
    }

    public class StepLabel<TElement> : StepLabel
    {
        public static implicit operator TElement(StepLabel<TElement>? stepLabel) => ThrowConversion();

        public static bool operator ==(TElement? a, StepLabel<TElement>? b) => ThrowEquality();

        public static bool operator !=(TElement? a, StepLabel<TElement>? b) => ThrowEquality();

        public static bool operator ==(StepLabel<TElement>? b, TElement? a) => ThrowEquality();

        public static bool operator !=(StepLabel<TElement>? b, TElement? a) => ThrowEquality();

        public TElement Value
        {
            get => ThrowConversion();
        }

        private static bool ThrowEquality() => throw new NotImplementedException($"The equality/inequality operators on {nameof(StepLabel)} are not intended to be called. Their use is to appear in expressions only.");

        private static TElement ThrowConversion() => throw new NotImplementedException($"The conversion operator on {nameof(StepLabel)} is not intended to be called. It's use is to appear in expressions only.");
    }

    // ReSharper disable once UnusedTypeParameter
    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQueryBase
    {
        
    }
}
#pragma warning restore 660,661
