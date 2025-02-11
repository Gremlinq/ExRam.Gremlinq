#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Security.Cryptography;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal sealed class StringGremlinQuery : GremlinQueryBase<string, object, object, IGremlinQueryBase>,
        IStringGremlinQuery
    {
        public StringGremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {
        }

        IStringGremlinQuery IStringGremlinQuery.Concat(params string[] strings) => this
            .Continue()
            .Build(
                static (builder, strings) => builder
                    .AddStep(new ConcatStringsStep(strings.ToImmutableArray()))
                    .As<StringGremlinQuery>(),
                strings);

        IStringGremlinQuery IStringGremlinQuery.Concat(params Func<IStringGremlinQuery, IGremlinQueryBase<string>>[] stringTraversals) => this
            .Continue()
            .With(stringTraversals)
            .Build(static (builder, stringTraversals) => builder
                .AddStep(new ConcatTraversalsStep(stringTraversals
                    .ToImmutableArray()))
                .As<StringGremlinQuery>());

        IStringGremlinQuery IStringGremlinQuery.Substring(Index startIndex) => Substring(Range.StartAt(startIndex));

        IStringGremlinQuery IStringGremlinQuery.Substring(Index startIndex, int length) => startIndex.IsFromEnd
            ? Substring(new Range(startIndex, Index.FromEnd(Math.Max(startIndex.Value - length, 0))))
            : Substring(new Range(startIndex, Index.FromStart(startIndex.Value + length)));

        IStringGremlinQuery IStringGremlinQuery.Substring(Range range) => Substring(range);

        private StringGremlinQuery Substring(Range range) => this
            .Continue()
            .Build(
                static (builder, range) => builder
                    .AddStep(new SubstringStep(range))
                    .As<StringGremlinQuery>(),
                range);

        private ContinuationBuilder<StringGremlinQuery, StringGremlinQuery> Continue() => new(
            this,
            new StringGremlinQuery(Environment, Traversal.Empty.WithProjection(Steps.Projection), LabelProjections, Metadata), ContinuationFlags.None);
    }
}
