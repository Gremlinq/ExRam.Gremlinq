using System.Text.RegularExpressions;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Provides a default <see cref="IPFactory"/> implementation and composition utilities.</summary>
    public static class PFactory
    {
        private sealed class DefaultPFactory : IPFactory
        {
            private static readonly P PNeqNull = P.Neq(null);

            public P? TryGetP(ExpressionSemantics semantics, object? maybeValue, IGremlinQueryEnvironment environment)
            {
                switch (semantics)
                {
                    case ContainsExpressionSemantics:
                    {
                        return new P("eq", maybeValue);
                    }
                    case IntersectsExpressionSemantics when maybeValue != null:
                    {
                        return P.Within(maybeValue);
                    }
                    case IsContainedInExpressionSemantics when maybeValue != null:
                    {
                        return P.Within(maybeValue);
                    }
                    case StringExpressionSemantics stringExpressionSemantics when maybeValue is string stringValue:
                    {
                        if (stringValue.Length == 0 || stringExpressionSemantics.Comparison == StringComparison.Ordinal)
                        {
                            switch (stringExpressionSemantics)
                            {
                                case StringEqualsExpressionSemantics:
                                {
                                    return new P("eq", stringValue);
                                }
                                case IsPrefixOfExpressionSemantics:
                                {
                                    return P.Within(SubStrings(stringValue));
                                }
                                case HasInfixExpressionSemantics:
                                {
                                    return stringValue.Length > 0
                                        ? TextP.Containing(stringValue)
                                        : PNeqNull;
                                }
                                case StartsWithExpressionSemantics:
                                {
                                    return stringValue.Length > 0
                                        ? TextP.StartingWith(stringValue)
                                        : PNeqNull;
                                }
                                case EndsWithExpressionSemantics:
                                {
                                    return stringValue.Length > 0
                                        ? TextP.EndingWith(stringValue)
                                        : PNeqNull;
                                }
                            }
                        }
                        else if (!environment.Options.GetValue(GremlinqOption.DisabledTextPredicates).HasFlag(DisabledTextPredicates.Regex))
                        {
                            switch (stringExpressionSemantics)
                            {
                                case StringEqualsExpressionSemantics:
                                {
                                    return TextP.Regex($"(?i)^{Regex.Escape(stringValue)}$");
                                }
                                case IsPrefixOfExpressionSemantics:
                                {
                                    return TextP.Regex($"(?i)^{string.Join('|', SubStrings(stringValue).Select(x => $"({Regex.Escape((string)x)})"))}$");
                                }
                                case HasInfixExpressionSemantics:
                                {
                                    return TextP.Regex($"(?i){Regex.Escape(stringValue)}");
                                }
                                case StartsWithExpressionSemantics:
                                {
                                    return TextP.Regex($"(?i)^{Regex.Escape(stringValue)}");
                                }
                                case EndsWithExpressionSemantics:
                                {
                                    return TextP.Regex($"(?i){Regex.Escape(stringValue)}$");
                                }
                            }
                        }

                        break;
                    }
                    case EqualsExpressionSemantics:
                    {
                        return new P("eq", maybeValue);
                    }
                    case NotEqualsExpressionSemantics:
                    {
                        return new P("neq", maybeValue);
                    }
                    case ObjectExpressionSemantics:
                    {
                        switch (semantics)
                        {
                            case LowerThanExpressionSemantics:
                            {
                                return new P("lt", maybeValue);
                            }
                            case GreaterThanExpressionSemantics:
                            {
                                return new P("gt", maybeValue);
                            }
                            case GreaterThanOrEqualExpressionSemantics:
                            {
                                return new P("gte", maybeValue);
                            }
                            case LowerThanOrEqualExpressionSemantics:
                            {
                                return new P("lte", maybeValue);
                            }
                        }

                        break;
                    }
                }

                return null;
            }

            private static object[] SubStrings(string value)
            {
                var ret = new object[value.Length + 1];

                for (var i = 0; i < ret.Length; i++)
                {
                    ret[i] = value[..i];
                }

                return ret;
            }
        }

        private sealed class OverridePFactory : IPFactory
        {
            private readonly IPFactory _originalFactory;
            private readonly IPFactory _overrideFactory;

            public OverridePFactory(IPFactory originalFactory, IPFactory overrideFactory)
            {
                _originalFactory = originalFactory;
                _overrideFactory = overrideFactory;
            }

            public P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment) => _overrideFactory.TryGetP(semantics, value, environment) ?? _originalFactory.TryGetP(semantics, value, environment);
        }

        /// <summary>Gets the default P factory implementation.</summary>
        public static readonly IPFactory Default = new DefaultPFactory();

        /// <summary>Creates a composite factory that tries the override factory first, falling back to the original.</summary>
        /// <param name="originalFactory">The original factory.</param>
        /// <param name="overrideFactory">The override factory that is tried first.</param>
        public static IPFactory Override(this IPFactory originalFactory, IPFactory overrideFactory)
        {
            ArgumentNullException.ThrowIfNull(originalFactory);
            ArgumentNullException.ThrowIfNull(overrideFactory);

            return new OverridePFactory(originalFactory, overrideFactory);
        }

        /// <summary>The option key for configuring the <see cref="IPFactory"/> on a <see cref="IGremlinQueryEnvironment"/>.</summary>
        public static readonly GremlinqOption<IPFactory> PFactoryOption = GremlinqOption.Create(Default);
    }
}
