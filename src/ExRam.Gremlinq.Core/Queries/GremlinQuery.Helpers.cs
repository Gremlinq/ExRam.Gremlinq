#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        internal GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Clone(
            Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment>? maybeEnvironmentTransformation = null,
            Func<StepStack, StepStack>? maybeStepStackTransformation = null,
            Func<Projection, Projection>? maybeProjectionTransformation = null,
            Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? maybeStepLabelProjectionsTransformation = null,
            Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? maybeSideEffectLabelProjectionsTransformation = null,
            Func<QueryFlags, QueryFlags>? maybeQueryFlagsTransformation = null)
        {
            return CloneAs<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>(
                maybeEnvironmentTransformation,
                maybeStepStackTransformation,
                maybeProjectionTransformation,
                maybeStepLabelProjectionsTransformation,
                maybeSideEffectLabelProjectionsTransformation,
                maybeQueryFlagsTransformation);
        }

        private IEnumerable<PropertyStep> GetPropertySteps(Key key, object value, bool allowExplicitCardinality)
        {
            if (value is not Traversal && value is IEnumerable enumerable && !Environment.GetCache().FastNativeTypes.ContainsKey(value.GetType()))
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
            object? actualValue = value;
            var metaProperties = ImmutableArray<KeyValuePair<string, object>>.Empty;

            if (actualValue is Property property)
            {
                if (property is IVertexProperty vertexProperty)
                {
                    metaProperties = vertexProperty.GetProperties(Environment)
                        .ToImmutableArray();
                }

                actualValue = property.GetValue();
            }

            return actualValue != null
                ? new PropertyStep.ByKeyStep(key, actualValue, metaProperties, cardinality)
                : null;
        }

        private ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> Continue() => new(
            this,
            Clone(
                maybeStepStackTransformation: _ => StepStack.Empty,
                maybeQueryFlagsTransformation: flags => (flags & ~QueryFlags.SurfaceVisible) | QueryFlags.IsAnonymous));

        private Key GetKey(Expression projection) => Environment.GetKey(projection);

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
    }
}
