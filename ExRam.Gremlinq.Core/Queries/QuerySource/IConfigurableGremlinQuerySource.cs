using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public enum FilterLabelsVerbosity
    {
        // Includes all appropriate labels in a map-step, e.g. ".out('l1', ...)", even if the set
        // of labels equals all possible labels, enabling the much shorter syntax ".out()".
        // This is the default and pessimistic // option since in an actual database, there might
        // exist elements with labels unknown to Gremlinq that would otherwise be incorrectly included
        // in the query results.
        Maximum = 0,

        // Assume there are no elements with labels unknown to Gremlinq in the queried database,
        // enabling Gremlinq to use e.g. "out()" when the more verbose syntax ".out('l1'...)" would
        // include all labels known to Gremlinq.
        Minimum = 1
    }

    public sealed class Options
    {
        public static readonly Options Default = new Options(FilterLabelsVerbosity.Maximum);

        public Options(FilterLabelsVerbosity filterLabelsVerbosity)
        {
            FilterLabelsVerbosity = filterLabelsVerbosity;
        }

        public FilterLabelsVerbosity FilterLabelsVerbosity { get; }

        public Options SetFilterLabelsVerbosity(FilterLabelsVerbosity value)
        {
            return new Options(value);
        }
    }

    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource
    {
        IConfigurableGremlinQuerySource WithName(string name);
        IConfigurableGremlinQuerySource WithLogger(ILogger logger);
        IConfigurableGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies);
        IConfigurableGremlinQuerySource ConfigureOptions(Func<Options, Options> optionsTransformation);
        IConfigurableGremlinQuerySource ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IConfigurableGremlinQuerySource ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);

        string Name { get; }
        ILogger Logger { get; }
        Options Options { get; }
        IGraphModel Model { get; }
        IGremlinQueryExecutor Executor { get; }
        ImmutableList<string> ExcludedStrategyNames { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
