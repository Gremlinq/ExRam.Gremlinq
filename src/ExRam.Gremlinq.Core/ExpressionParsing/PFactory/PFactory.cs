using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public static class PFactory
    {
        private sealed class DefaultPFactory : IPFactory
        {
            private static readonly P PNeqNull = P.Neq(new object?[] { null });

            public P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment)
            {
                switch (semantics)
                {
                    case ContainsExpressionSemantics:
                    {
                        return new P("eq", value);
                    }
                    case IntersectsExpressionSemantics:
                    {
                        return P.Within(value);
                    }
                    case IsContainedInExpressionSemantics:
                    {
                        return P.Within(value);
                    }
                    case StringExpressionSemantics stringExpressionSemantics when value is string stringValue:
                    {
                        if (stringExpressionSemantics.Comparison == StringComparison.Ordinal || environment.Options.GetValue(GremlinqOption.StringComparisonTranslationStrictness) == StringComparisonTranslationStrictness.Lenient)
                        {
                            switch (stringExpressionSemantics)
                            {
                                case StringEqualsExpressionSemantics:
                                {
                                    return new P("eq", value);
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
                        return new P("eq", value);
                    }
                    case NotEqualsExpressionSemantics:
                    {
                        return new P("neq", value);
                    }
                    case ObjectExpressionSemantics:
                    {
                        switch (semantics)
                        {
                            case LowerThanExpressionSemantics:
                            {
                                return new P("lt", value);
                            }
                            case GreaterThanExpressionSemantics:
                            {
                                return new P("gt", value);
                            }
                            case GreaterThanOrEqualExpressionSemantics:
                            {
                                return new P("gte", value);
                            }
                            case LowerThanOrEqualExpressionSemantics:
                            {
                                return new P("lte", value);
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

        public static readonly GremlinqOption<IPFactory> PFactoryOption = new(Default);
    }
}
