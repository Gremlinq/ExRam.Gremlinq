namespace ExRam.Gremlinq.Core
{
    public struct Options
    {
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
}
