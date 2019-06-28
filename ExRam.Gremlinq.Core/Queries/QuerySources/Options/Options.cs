namespace ExRam.Gremlinq.Core
{
    public struct Options
    {
        public Options(FilterLabelsVerbosity filterLabelsVerbosity)
        {
            FilterLabelsVerbosity = filterLabelsVerbosity;
        }

        public Options SetFilterLabelsVerbosity(FilterLabelsVerbosity value)
        {
            return new Options(
                filterLabelsVerbosity: value);
        }

        public FilterLabelsVerbosity FilterLabelsVerbosity { get; }
    }
}
