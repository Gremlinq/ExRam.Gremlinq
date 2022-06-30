using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Linq;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    public static class ImmutableDictionaryExtensions
    {
        private static readonly ConcurrentDictionary<object, object> FastDictionaries = new();

        internal static IImmutableDictionary<MemberInfo, MemberMetadata> ConfigureNames(this IImmutableDictionary<MemberInfo, MemberMetadata> metadata, Func<MemberInfo, Key, Key> transformation)
        {
            return metadata
                .SetItems(metadata
                    .Select(kvp => new KeyValuePair<MemberInfo, MemberMetadata>(
                        kvp.Key,
                        new MemberMetadata(
                            transformation(kvp.Key, kvp.Value.Key),
                            kvp.Value.SerializationBehaviour))));
        }

        internal static IReadOnlyDictionary<TKey, TValue> Fast<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dict)
            where TKey : notnull
        {
            return (IReadOnlyDictionary<TKey, TValue>)FastDictionaries
                .GetOrAdd(
                    dict,
                    static closureDict => ((IImmutableDictionary<TKey, TValue>)closureDict).ToDictionary(static x => x.Key, static x => x.Value));
        }
        
        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseCamelCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames(static (_, key) => key.RawKey is string name
                ? name.ToCamelCase()
                : key);
        }

        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseLowerCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames(static (_, key) => key.RawKey is string name
                ? name.ToLower()
                : key);
        }

        internal static IImmutableDictionary<TKey, TValue> Set<TKey, TValue, TState>(this IImmutableDictionary<TKey, TValue> dict, TKey key, TState state, Func<TValue, TState, TValue> change)
            where TValue : new()
        {
            return dict.Set(
                key,
                (change, state),
                static state => state.change(new TValue(), state.state),
                static (value, state) => state.change(value, state.state));
        }

        internal static IImmutableDictionary<TKey, TValue> Set<TKey, TValue, TState>(this IImmutableDictionary<TKey, TValue> dict, TKey key, TState state, Func<TState, TValue> create, Func<TValue, TState, TValue> change)
        {
            var newValue = dict.TryGetValue(key, out var value)
                ? change(value, state)
                : create(state);

            return dict.SetItem(key, newValue);
        }

        internal static IImmutableDictionary<StepLabel, LabelProjections> MergeSideEffectLabelProjections(this IImmutableDictionary<StepLabel, LabelProjections> projections, IImmutableDictionary<StepLabel, LabelProjections> newProjections)
        {
            return projections
                .MergeLabelProjections(
                    newProjections,
                    static projections => projections.SideEffectLabelProjection,
                    static (projections, projection) => projections.WithSideEffectLabelProjection(projection));
        }

        internal static IImmutableDictionary<StepLabel, LabelProjections> MergeStepLabelProjections(this IImmutableDictionary<StepLabel, LabelProjections> projections, IImmutableDictionary<StepLabel, LabelProjections> newProjections)
        {
            return projections
                .MergeLabelProjections(
                    newProjections,
                    static projections => projections.StepLabelProjection,
                    static (projections, projection) => projections.WithStepLabelProjection(projection));
        }

        internal static IImmutableDictionary<StepLabel, LabelProjections> MergeLabelProjections(this IImmutableDictionary<StepLabel, LabelProjections> projections, IImmutableDictionary<StepLabel, LabelProjections> newProjections, Func<LabelProjections, Projection?> projectionMap, Func<LabelProjections, Projection, LabelProjections> projectionMapper)
        {
            foreach (var kvp in newProjections)
            {
                if (projectionMap(kvp.Value) is { } newLabelProjection)
                {
                    var mappedProjections = projectionMapper(kvp.Value, newLabelProjection);

                    projections = projections.SetItem(
                        kvp.Key,
                        mappedProjections);
                }
            }

            return projections;
        }
    }
}
