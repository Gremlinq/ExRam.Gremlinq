#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.Projections;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
    {
        private Cardinality? GetCardinality(object value, bool allowExplicitCardinality) => allowExplicitCardinality
            ? (value is not Traversal && value is IEnumerable && !Environment.SupportsType(value.GetType()))
                ? Cardinality.List
                : Cardinality.Single
            : null;

        private ContinuationBuilder<GremlinQuery<T1, T2, T3, T4>> Continue(ContinuationFlags flags = ContinuationFlags.None) => Continue<T1, T2, T3, T4>(flags);

        private ContinuationBuilder<GremlinQuery<TAnon1, TAnon2, TAnon3, TAnon4>> Continue<TAnon1, TAnon2, TAnon3, TAnon4>(ContinuationFlags flags = ContinuationFlags.None) where TAnon4 : IGremlinQueryBase => new (
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

        private ImmutableArray<string> GetStringKeyArray(ReadOnlySpan<LambdaExpression> projections)
        {
            if (projections is [])
                return ImmutableArray<string>.Empty;

            var stringKeys = new string[projections.Length];

            for (var i = 0; i < projections.Length; i++)
            {
                if (GetKey(projections[i]).RawKey is string stringKey)
                    stringKeys[i] = stringKey;
                else
                    throw new ExpressionNotSupportedException(projections[i]);
            }

            return stringKeys
                .UnsafeToImmutableArray();
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
