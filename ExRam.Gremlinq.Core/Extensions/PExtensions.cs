using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class PExtensions
    {
        public static bool ContainsOnlyStepLabels(this P p)
        {
            return p.OperatorName switch
            {
                "and" => (((P)p.Value).ContainsOnlyStepLabels() && p.Other.ContainsOnlyStepLabels()),
                "or" => (((P)p.Value).ContainsOnlyStepLabels() && p.Other.ContainsOnlyStepLabels()),
                _ => p.Value is StepLabel || (p.Value is Expression expression && typeof(StepLabel).IsAssignableFrom(expression.Type))
            };
        }

        public static bool EqualsConstant(this P p, bool value)
        {
            return p.OperatorName switch
            {
                "within" => !value && p.Value is IEnumerable enumerable && !enumerable.InternalAny(),
                "without" => value && p.Value is IEnumerable enumerable && !enumerable.InternalAny(),
                "and" => value
                    ? ((P)p.Value).EqualsConstant(true) && p.Other.EqualsConstant(true)
                    : ((P)p.Value).EqualsConstant(false) || p.Other.EqualsConstant(false),
                "or" => value
                    ? ((P)p.Value).EqualsConstant(true) || p.Other.EqualsConstant(true)
                    : ((P)p.Value).EqualsConstant(false) && p.Other.EqualsConstant(false),
                _ => false
            };
        }

        public static P WorkaroundLimitations(this TextP textP, IImmutableDictionary<GremlinqOption, object> gremlinqOptions)
        {
            switch (textP.OperatorName)
            {
                case "startingWith":
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
                case "endingWith" when (gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.EndingWith) != 0:
                    throw new ExpressionNotSupportedException($"Can't work around {nameof(TextP.EndingWith)} without the use of {nameof(TextP)} predicates.");
                case "endingWith":
                    return textP;
                case "containing" when (gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.Containing) != 0:
                    throw new ExpressionNotSupportedException($"Can't work around {nameof(TextP.Containing)} without the use of {nameof(TextP)} predicates.");
                case "containing":
                    return textP;
                default:
                    return textP;
            }
        }
    }
}
