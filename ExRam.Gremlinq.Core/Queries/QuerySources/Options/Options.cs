namespace ExRam.Gremlinq.Core
{
    public struct Options
    {
        public Options(
            FilterLabelsVerbosity filterLabelsVerbosity,
            DisabledTextPredicates disabledTextPredicates)
        {
            FilterLabelsVerbosity = filterLabelsVerbosity;
            DisabledTextPredicates = disabledTextPredicates;
        }

        public Options SetFilterLabelsVerbosity(FilterLabelsVerbosity value)
        {
            return new Options(
                filterLabelsVerbosity: value,
                disabledTextPredicates: DisabledTextPredicates);
        }

        public Options SetDisabledTextPredicates(DisabledTextPredicates value)
        {
            return new Options(
                filterLabelsVerbosity: FilterLabelsVerbosity,
                disabledTextPredicates: value);
        }

        public FilterLabelsVerbosity FilterLabelsVerbosity { get; }
        public DisabledTextPredicates DisabledTextPredicates { get; }
    }
}
