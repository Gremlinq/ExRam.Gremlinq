using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class PExtensions
    {
        public static bool ContainsOnlyStepLabels(this P p)
        {
            switch (p.OperatorName)
            {
                case "and":
                case "or":
                    return ((P)p.Value).ContainsOnlyStepLabels() && p.Other.ContainsOnlyStepLabels();
                default:
                    return p.Value is StepLabel || p.Value is IEnumerable enumerable && enumerable.Cast<object>().Any() && enumerable.Cast<object>().All(x => x is StepLabel);
            }
        }

        public static bool EqualsConstant(this P p, bool value)
        {
            switch (p.OperatorName)
            {
                case "within":
                {
                    return !value && p.Value is IEnumerable enumerable && !enumerable.Cast<object>().Any(); //TODO: Any extension;
                }
                case "without":
                {
                    return value && p.Value is IEnumerable enumerable && !enumerable.Cast<object>().Any(); //TODO: Any extension
                }
                case "and":
                {
                    return value
                        ? ((P)p.Value).EqualsConstant(true) && p.Other.EqualsConstant(true)
                        : ((P)p.Value).EqualsConstant(false) || p.Other.EqualsConstant(false);
                }
                case "or":
                {
                    return value
                        ? ((P)p.Value).EqualsConstant(true) || p.Other.EqualsConstant(true)
                        : ((P)p.Value).EqualsConstant(false) && p.Other.EqualsConstant(false);
                }
                default:
                    return false;
            }
        }

        public static P WorkaroundLimitations(this TextP textP, IImmutableDictionary<GremlinqOption, object> gremlinqOptions)
        {
            if (textP.OperatorName == "startingWith")
            {
                var value = (string)textP.Value;

                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.StartingWith) == 0)
                    return textP;

                string upperBound;

                if (value[value.Length - 1] == char.MaxValue)
                    upperBound = value + char.MinValue;
                else
                {
                    var upperBoundChars = value.ToCharArray();

                    upperBoundChars[upperBoundChars.Length - 1]++;
                    upperBound = new string(upperBoundChars);
                }

                return P.Between(value, upperBound);
            }

            if (textP.OperatorName == "endingWith")
            {
                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.EndingWith) != 0)
                    throw new ExpressionNotSupportedException();

                return textP;
            }

            if (textP.OperatorName == "containing")
            {
                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.Containing) != 0)
                    throw new ExpressionNotSupportedException();

                return textP;
            }

            return textP;
        }
    }
}
