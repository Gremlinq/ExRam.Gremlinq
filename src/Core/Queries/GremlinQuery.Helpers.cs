#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

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

            return memberExpression.IsPropertyValue(out var sourceExpression) && sourceExpression is MemberExpression
                ? GetKey(sourceExpression)
                : Environment
                    .GetCache()
                    .GetMetadata(memberExpression.Member)
                    .Key;
        }

        private ImmutableArray<string> GetStringKeys(ReadOnlySpan<LambdaExpression> projections)
        {
            var stringKeys = new string[projections.Length];

            for (var i = 0; i < projections.Length; i++)
            {
                if (GetKey(projections[i]).RawKey is string stringKey)
                    stringKeys[i] = stringKey;
                else
                    throw new ExpressionNotSupportedException(projections[i]);
            }

#if NET8_0_OR_GREATER
            return ImmutableCollectionsMarshal.AsImmutableArray(stringKeys);
#else
            return stringKeys.ToImmutableArray();
#endif
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
