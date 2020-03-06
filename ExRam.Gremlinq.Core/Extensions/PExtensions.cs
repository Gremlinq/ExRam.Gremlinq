using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class PExtensions
    {
        public static bool RefersToStepLabel(this P p)
        {
            return p.OperatorName switch
            {
                "and" => (((P)p.Value).RefersToStepLabel() && p.Other.RefersToStepLabel()),
                "or" => (((P)p.Value).RefersToStepLabel() && p.Other.RefersToStepLabel()),
                _ => ((object)p.Value).RefersToStepLabel()
            };
        }

        private static bool RefersToStepLabel(this object obj)
        {
            return obj is StepLabel
                || obj is IList list && list.Count == 1 && list[0].RefersToStepLabel()
                || obj is Expression expression && expression.TryParseStepLabelExpression(out _, out _);
        }

        public static P Resolve(this P p)
        {
            return new P(
                p.OperatorName,
                p.Value switch
                {
                    string s => s,
                    IEnumerable enumerable => enumerable
                        .Cast<object>()
                        .Select(o => o switch
                        {
                            Expression e => e.GetValue(),
                            var v => v
                        })
                        .SelectMany(x => x switch
                        {
                            string s => new[] { s },
                            IEnumerable e => e.Cast<object>(),
                            var v => new[] { v }
                        })
                        .ToArray(),
                    var v => v
                },
                p.Other?.Resolve());
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
