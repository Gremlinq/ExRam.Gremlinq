using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinqOption
    {
        public static GremlinqOption<TValue> Create<TValue>(TValue defaultValue) => GremlinqOption<TValue>.Create(defaultValue);

        public static readonly GremlinqOption<Traversal> VertexPropertyProjectionSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "value", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByKeyStep(T.Value),
            new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))));

        public static readonly GremlinqOption<Traversal> VertexProjectionSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByTraversalStep(Traversal.Empty.Push(
                new PropertiesStep(ImmutableArray<string>.Empty),
                GroupStep.Instance,
                new GroupStep.ByKeyStep(T.Label),
                new GroupStep.ByTraversalStep(VertexPropertyProjectionSteps.DefaultValue.Push(
                    FoldStep.Instance))))));

        public static readonly GremlinqOption<Traversal> VertexProjectionWithoutMetaPropertiesSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByTraversalStep(Traversal.Empty.Push(
                new PropertiesStep(ImmutableArray<string>.Empty),
                GroupStep.Instance,
                new GroupStep.ByKeyStep(T.Label),
                new GroupStep.ByTraversalStep(Traversal.Empty.Push(
                    new ProjectStep(ImmutableArray.Create("id", "label", "value")),
                    new ProjectStep.ByKeyStep(T.Id),
                    new ProjectStep.ByKeyStep(T.Label),
                    new ProjectStep.ByKeyStep(T.Value),
                    FoldStep.Instance))))));

        public static readonly GremlinqOption<Traversal> VertexPropertyProjectionWithoutMetaPropertiesSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "value")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByKeyStep(T.Value)));

        public static readonly GremlinqOption<Traversal> EdgeProjectionSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))));

        public static readonly GremlinqOption<Traversal> EmptyProjectionProtectionDecoratorSteps = Create(Traversal.Empty.Push(
            new MapStep(Traversal.Empty.Push(
                UnfoldStep.Instance,
                GroupStep.Instance,
                new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Keys)),
                new GroupStep.ByTraversalStep(Traversal.Empty.Push(
                    new SelectColumnStep(Column.Values),
                    UnfoldStep.Instance))))));

        public static readonly GremlinqOption<IImmutableDictionary<T, SerializationBehaviour>> TSerializationBehaviourOverrides = Create<IImmutableDictionary<T, SerializationBehaviour>>(
            new Dictionary<T, SerializationBehaviour>
            {
                { T.Key, SerializationBehaviour.IgnoreOnUpdate },
                { T.Id, SerializationBehaviour.IgnoreOnUpdate },
                { T.Label, SerializationBehaviour.IgnoreAlways },
                { T.Value, SerializationBehaviour.Default }
            }
            .ToImmutableDictionary());

        public static readonly GremlinqOption<bool> WorkaroundRangeInconsistencies = Create(false);

        public static readonly GremlinqOption<string> Alias = Create("g");

        public static readonly GremlinqOption<bool> EnableEmptyProjectionValueProtection = Create(false);

        public static readonly GremlinqOption<Func<StepLabel, Projection>> StepLabelProjectionFallback = Create<Func<StepLabel, Projection>>(
            static _ => throw new InvalidOperationException($"Invalid use of unknown {nameof(StepLabel)} in {nameof(IGremlinQueryBase.Select)}. Make sure you only pass in a {nameof(StepLabel)} that comes from a previous {nameof(IGremlinQuery<int>.As)}, {nameof(IGremlinQuery<int>.Aggregate)} or {nameof(IGremlinQuerySource.WithSideEffect)}-continuation or has previously been passed to an appropriate overload of {nameof(IGremlinQuery<int>.As)}, {nameof(IGremlinQuery<int>.Aggregate)} or {nameof(IGremlinQuerySource.WithSideEffect)}."));

        public static readonly GremlinqOption<FilterLabelsVerbosity> FilterLabelsVerbosity = Create(Core.FilterLabelsVerbosity.Maximum);
        public static readonly GremlinqOption<DisabledTextPredicates> DisabledTextPredicates = Create(Core.DisabledTextPredicates.None);
        public static readonly GremlinqOption<StringComparisonTranslationStrictness> StringComparisonTranslationStrictness = Create(Core.StringComparisonTranslationStrictness.Strict);

        public static readonly GremlinqOption<LogLevel> QueryLogLogLevel = Create(LogLevel.Debug);
        public static readonly GremlinqOption<QueryLogFormatting> QueryLogFormatting = Create(Core.QueryLogFormatting.None);
        public static readonly GremlinqOption<QueryLogVerbosity> QueryLogVerbosity = Create(Core.QueryLogVerbosity.QueryOnly);
    }

    public sealed class GremlinqOption<TValue>
    {
        private GremlinqOption(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; }

        internal static GremlinqOption<TValue> Create(TValue defaultValue) => new(defaultValue);
    }
}
