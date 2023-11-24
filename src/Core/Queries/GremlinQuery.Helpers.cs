#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private IEnumerable<PropertyStep> GetPropertySteps(Key key, object value, bool allowExplicitCardinality)
        {
            if (value is not Traversal && value is IEnumerable enumerable && !Environment.SupportsType(value.GetType()))
            {
                if (!allowExplicitCardinality)
                    throw new NotSupportedException($"A value of type {value.GetType()} is not supported for property '{key}'.");

                foreach (var item in enumerable)
                {
                    if (TryGetPropertyStep(key, item, Cardinality.List) is { } step)
                        yield return step;
                }
            }
            else
            {
                if (TryGetPropertyStep(key, value, allowExplicitCardinality ? Cardinality.Single : default) is { } step)
                    yield return step;
            }
        }

        private PropertyStep? TryGetPropertyStep(Key key, object value, Cardinality? cardinality)
        {
            var actualValue = value;
            var metaProperties = ImmutableArray<KeyValuePair<string, object>>.Empty;

            if (actualValue is Property property)
            {
                if (property is IVertexProperty vertexProperty)
                {
                    metaProperties = vertexProperty
                        .GetProperties(Environment)
                        .Select(static kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value))
                        .ToImmutableArray();
                }

                actualValue = property.GetValue();
            }

            return actualValue != null
                ? new PropertyStep.ByKeyStep(key, actualValue, metaProperties, cardinality)
                : null;
        }

        private ContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>> Continue(ContinuationFlags flags = ContinuationFlags.None) => Continue<T1, T2, T3, T4>(flags);

        private ContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<TAnon1, TAnon2, TAnon3, TAnon4>> Continue<TAnon1, TAnon2, TAnon3, TAnon4>(ContinuationFlags flags = ContinuationFlags.None) where TAnon4 : IGremlinQueryBase => new (
            this,
            new GremlinQuery<TAnon1, TAnon2, TAnon3, TAnon4>(Environment, Traversal.Empty.WithProjection(Steps.Projection), LabelProjections, Metadata), flags);

        private Key GetKey(Expression expression)
        {
            var memberExpression = expression
                .AssumeMemberExpression();

            return Environment
                .GetCache()
                .GetMetadata(memberExpression.TryGetWellKnownMember() == WellKnownMember.PropertyValue && memberExpression.Expression is MemberExpression sourceMemberExpression
                    ? sourceMemberExpression.Member
                    : memberExpression.Member)
                .Key;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumerable<Step> GetStepsForKeys(IEnumerable<Key> keys)
        {
            var hasYielded = false;
            var stringKeys = default(List<string>?);

            foreach (var key in keys)
            {
                switch (key.RawKey)
                {
                    case T t:
                        {
                            if (t.TryToStep() is { } step)
                                yield return step;
                            else
                                throw new ExpressionNotSupportedException($"Can't find an appropriate Gremlin step for {t}.");

                            hasYielded = true;

                            break;
                        }
                    case string str:
                        {
                            (stringKeys ??= new List<string>()).Add(str);

                            break;
                        }
                    default:
                        throw new ExpressionNotSupportedException($"Can't find an appropriate Gremlin step for {key.RawKey}.");
                }
            }

            if (stringKeys?.Count > 0 || !hasYielded)
                yield return new ValuesStep(stringKeys?.ToImmutableArray() ?? ImmutableArray<string>.Empty);
        }

        private IEnumerable<string> GetStringKeys(Expression[] projections)
        {
            foreach (var projection in projections)
            {
                if (GetKey(projection).RawKey is string str)
                    yield return str;
            }
        }

        private Projection GetLabelProjection(StepLabel stepLabel)
        {
            LabelProjections.TryGetValue(stepLabel, out var projections);

            return projections.StepLabelProjection
                ?? projections.SideEffectLabelProjection
                ?? Environment.Options.GetValue(GremlinqOption.StepLabelProjectionFallback)(stepLabel);
        }
    }
}
