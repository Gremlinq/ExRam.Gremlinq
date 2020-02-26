using System.Collections;
using System.Collections.Immutable;
using System.Linq;
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
                _ => (p.Value is StepLabel || p.Value is IEnumerable enumerable && enumerable.Cast<object>().Any() && enumerable.Cast<object>().All(x => x is StepLabel))
            };
        }

        public static P Fuse(this P p1, P p2, ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.AndAlso => p1.And(p2),
                ExpressionType.OrElse => p1.Or(p2),
                _ => throw new ExpressionNotSupportedException()
            };
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
                    throw new ExpressionNotSupportedException();
                case "endingWith":
                    return textP;
                case "containing" when (gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.Containing) != 0:
                    throw new ExpressionNotSupportedException();
                case "containing":
                    return textP;
                default:
                    return textP;
            }
        }
    }
}
