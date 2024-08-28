﻿using System.Collections;
using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Steps;
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
                "within" => !value && p.Value is ICollection collection && collection.Count == 0,
                "without" => value && p.Value is ICollection collection && collection.Count > 0,
                "and" when p is { Value: P pValue, Other: { } otherP } => value
                    ? pValue.EqualsConstant(true) && otherP.EqualsConstant(true)
                    : pValue.EqualsConstant(false) || otherP.EqualsConstant(false),
                "or" when p is { Value: P pValue, Other: { } otherP } => value
                    ? pValue.EqualsConstant(true) || otherP.EqualsConstant(true)
                    : pValue.EqualsConstant(false) && otherP.EqualsConstant(false),
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
                    case "startingWith" when textP.Value is string stringValue:
                    {
                        if (stringValue.Length == 0)
                            return new P("true", default);

                        if ((disabledTextPredicates & DisabledTextPredicates.StartingWith) == 0)
                            return textP;

                        string upperBound;

                        if (stringValue[^1] == char.MaxValue)
                            upperBound = stringValue + char.MinValue;
                        else
                        {
                            var upperBoundChars = stringValue.ToCharArray();

                            upperBoundChars[^1]++;
                            upperBound = new string(upperBoundChars);
                        }

                        return P.Between(stringValue, upperBound);
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

        public static Step GetFilterStep(this P p, Key key)
        {
            if (p.ContainsNullArgument())
            {
                if ((p.IsAnd() || p.IsOr()) && p.Other is { } otherP)
                {
                    var replacement = ImmutableArray
                        .Create<Traversal>((p.Value as P ?? (P)P.Eq(p.Value)).GetFilterStep(key), otherP.GetFilterStep(key));

                    if (p.IsOr())
                        return new OrStep(replacement);

                    if (p.IsAnd())
                        return new AndStep(replacement);
                }
            }

            if (p.Value == null)
            {
                if (p.OperatorName == "eq")
                    return new HasNotStep(key);

                if (p.OperatorName is "neq" or "gt")
                    return new HasStep(key);
            }

            return new HasPredicateStep(key, p);
        }
    }
}
