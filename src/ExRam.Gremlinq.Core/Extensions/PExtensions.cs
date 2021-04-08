using System;
using System.Collections;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class PExtensions
    {
        public static bool EqualsConstant(this P p, bool value)
        {
            return p.OperatorName switch
            {
                //"containing" when p.Value is string str && str.Length == 0 => value,
                "within" => !value && p.Value is IEnumerable enumerable && !enumerable.InternalAny(),
                "without" => value && p.Value is IEnumerable enumerable && !enumerable.InternalAny(),
                "and" => value
                    ? ((P)p.Value).EqualsConstant(true) && p.Other.EqualsConstant(true)
                    : ((P)p.Value).EqualsConstant(false) || p.Other.EqualsConstant(false),
                "or" => value
                    ? ((P)p.Value).EqualsConstant(true) || p.Other.EqualsConstant(true)
                    : ((P)p.Value).EqualsConstant(false) && p.Other.EqualsConstant(false),
                "true" => value,
                "false" => !value,
                _ => false
            };
        }

        public static P WorkaroundLimitations(this P p, IGremlinQueryEnvironment environment)
        {
            var disabledTextPredicates = environment.Options.GetValue(GremlinqOption.DisabledTextPredicates);

            if (p is TextP textP)
            {
                switch (textP.OperatorName)
                {
                    case "startingWith":
                    {
                        var value = (string)textP.Value;

                        if (value.Length == 0)
                            return new P("true", default);

                        if ((disabledTextPredicates & DisabledTextPredicates.StartingWith) == 0)
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
                    case "endingWith" when (disabledTextPredicates & DisabledTextPredicates.EndingWith) != 0:
                        throw new ExpressionNotSupportedException($"Can't work around {nameof(TextP.EndingWith)} without the use of {nameof(TextP)} predicates.");
                    case "endingWith":
                        return textP;
                    case "containing" when (disabledTextPredicates & DisabledTextPredicates.Containing) != 0:
                        throw new ExpressionNotSupportedException($"Can't work around {nameof(TextP.Containing)} without the use of {nameof(TextP)} predicates.");
                    case "containing":
                        return textP;
                    default:
                        return textP;
                }
            }

            return p;
        }

        public static bool ContainsNullArgument(this P p) => p.Value == null || (p.Value is P firstP && ContainsNullArgument(firstP)) || (p.Other is { } otherP && ContainsNullArgument(otherP));

        public static bool IsAnd(this P p) => p.OperatorName.Equals("and", StringComparison.OrdinalIgnoreCase);

        public static bool IsOr(this P p) => p.OperatorName.Equals("or", StringComparison.OrdinalIgnoreCase);
    }
}
