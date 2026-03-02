using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides well-known <see cref="GremlinqOption{TValue}"/> instances for configuring query behavior.
    /// </summary>
    public static class GremlinqOption
    {
        /// <summary>
        /// Creates a new <see cref="GremlinqOption{TValue}"/> with the specified default value.
        /// </summary>
        /// <typeparam name="TValue">The type of the option's value.</typeparam>
        /// <param name="defaultValue">The default value for the option.</param>
        public static GremlinqOption<TValue> Create<TValue>(TValue defaultValue) => GremlinqOption<TValue>.Create(defaultValue);

        /// <summary>
        /// The traversal steps used for projecting vertex properties.
        /// </summary>
        public static readonly GremlinqOption<Traversal> VertexPropertyProjectionSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "value", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByKeyStep(T.Value),
            new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))));

        /// <summary>
        /// The traversal steps used for projecting vertices.
        /// </summary>
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

        /// <summary>
        /// The traversal steps used for projecting vertices when meta-properties are not supported.
        /// </summary>
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

        /// <summary>
        /// The traversal steps used for projecting vertex properties when meta-properties are not supported.
        /// </summary>
        public static readonly GremlinqOption<Traversal> VertexPropertyProjectionWithoutMetaPropertiesSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "value")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByKeyStep(T.Value)));

        /// <summary>
        /// The traversal steps used for projecting edges.
        /// </summary>
        public static readonly GremlinqOption<Traversal> EdgeProjectionSteps = Create(Traversal.Empty.Push(
            new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))));

        /// <summary>
        /// The traversal steps used for protecting against empty projections.
        /// </summary>
        public static readonly GremlinqOption<Traversal> EmptyProjectionProtectionDecoratorSteps = Create(Traversal.Empty.Push(
            new MapStep(Traversal.Empty.Push(
                UnfoldStep.Instance,
                GroupStep.Instance,
                new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Keys)),
                new GroupStep.ByTraversalStep(Traversal.Empty.Push(
                    new SelectColumnStep(Column.Values),
                    UnfoldStep.Instance))))));

        /// <summary>
        /// Overrides for the serialization behaviour of TinkerPop <see cref="Gremlin.Net.Process.Traversal.T"/> enum values.
        /// </summary>
        public static readonly GremlinqOption<IImmutableDictionary<T, SerializationBehaviour>> TSerializationBehaviourOverrides = Create<IImmutableDictionary<T, SerializationBehaviour>>(
            new Dictionary<T, SerializationBehaviour>
            {
                { T.Key, SerializationBehaviour.IgnoreOnUpdate },
                { T.Id, SerializationBehaviour.IgnoreOnUpdate },
                { T.Label, SerializationBehaviour.IgnoreAlways },
                { T.Value, SerializationBehaviour.Default }
            }
            .ToImmutableDictionary());

        /// <summary>
        /// Enables a workaround for range step inconsistencies across different graph databases.
        /// </summary>
        public static readonly GremlinqOption<bool> WorkaroundRangeInconsistencies = Create(false);

        /// <summary>
        /// The alias used for the traversal source in Groovy scripts. Defaults to <c>"g"</c>.
        /// </summary>
        public static readonly GremlinqOption<string> Alias = Create("g");

        /// <summary>
        /// Enables protection against empty projection values in query results.
        /// </summary>
        public static readonly GremlinqOption<bool> EnableEmptyProjectionValueProtection = Create(false);

        /// <summary>
        /// A fallback function that determines the projection for an unknown <see cref="StepLabel"/>.
        /// </summary>
        public static readonly GremlinqOption<Func<StepLabel, Projection>> StepLabelProjectionFallback = Create<Func<StepLabel, Projection>>(
            static _ => throw new InvalidOperationException($"Invalid use of unknown {nameof(StepLabel)} in {nameof(IGremlinQueryBase.Select)}. Make sure you only pass in a {nameof(StepLabel)} that comes from a previous {nameof(IGremlinQuery<>.As)}, {nameof(IGremlinQuery<>.Aggregate)} or {nameof(IGremlinQuerySource.WithSideEffect)}-continuation or has previously been passed to an appropriate overload of {nameof(IGremlinQuery<>.As)}, {nameof(IGremlinQuery<>.Aggregate)} or {nameof(IGremlinQuerySource.WithSideEffect)}."));

        /// <summary>
        /// Controls the verbosity of label filters in generated Gremlin queries.
        /// </summary>
        public static readonly GremlinqOption<FilterLabelsVerbosity> FilterLabelsVerbosity = Create(Core.FilterLabelsVerbosity.Maximum);

        /// <summary>
        /// Specifies which text predicates are disabled for the target database.
        /// </summary>
        public static readonly GremlinqOption<DisabledTextPredicates> DisabledTextPredicates = Create(Core.DisabledTextPredicates.None);

        [Obsolete("Starting from version 14, Gremlinq will always behave as if StringComparisonTranslationStrictness.Strict was configured. Queries using a string comparison which is not supported on a specific database provider (e.g. case insensitive queries on Azure CosmosDb) must be modified accordingly.")]
        public static readonly GremlinqOption<StringComparisonTranslationStrictness> StringComparisonTranslationStrictness = Create(Core.StringComparisonTranslationStrictness.Strict);

        /// <summary>
        /// The log level used when logging queries.
        /// </summary>
        public static readonly GremlinqOption<LogLevel> QueryLogLogLevel = Create(LogLevel.Debug);

        /// <summary>
        /// Controls how much detail is included when logging queries.
        /// </summary>
        public static readonly GremlinqOption<QueryLogVerbosity> QueryLogVerbosity = Create(Core.QueryLogVerbosity.QueryOnly);
    }

    /// <summary>
    /// Represents a typed configuration option for Gremlinq.
    /// </summary>
    /// <typeparam name="TValue">The type of the option's value.</typeparam>
    public sealed class GremlinqOption<TValue>
    {
        private GremlinqOption(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the default value for this option.
        /// </summary>
        public TValue DefaultValue { get; }

        internal static GremlinqOption<TValue> Create(TValue defaultValue) => new(defaultValue);
    }
}
