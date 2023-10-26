using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
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
                    case IntersectsExpressionSemantics when maybeValue is { } value:
                    {
                        return P.Within(value);
                    }
                    case IsContainedInExpressionSemantics when maybeValue is { } value:
                    {
                        return P.Within(value);
                    }
                    case StringExpressionSemantics stringExpressionSemantics when maybeValue is string stringValue:
                    {
                        if (stringValue.Length == 0 || stringExpressionSemantics.Comparison == StringComparison.Ordinal || environment.Options.GetValue(GremlinqOption.StringComparisonTranslationStrictness) == StringComparisonTranslationStrictness.Lenient)
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

                throw new ExpressionNotSupportedException();
            }

            private static object[] SubStrings(string value)
            {
                var ret = new object[value.Length + 1];

                for (var i = 0; i < ret.Length; i++)
                {
                    ret[i] = value.Substring(0, i);
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

        public static readonly IPFactory Default = new DefaultPFactory();

        public static IPFactory Override(this IPFactory originalFactory, IPFactory overrideFactory) => new OverridePFactory(originalFactory, overrideFactory);

        public static readonly GremlinqOption<IPFactory> PFactoryOption = GremlinqOption.Create(Default);
    }
}
