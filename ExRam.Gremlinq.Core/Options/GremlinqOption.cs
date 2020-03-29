namespace ExRam.Gremlinq.Core
{
    public class GremlinqOption
    {
        public static GremlinqOption<bool> DontAddElementProjectionSteps = new GremlinqOption<bool>(false);
        public static GremlinqOption<FilterLabelsVerbosity> FilterLabelsVerbosity = new GremlinqOption<FilterLabelsVerbosity>(Core.FilterLabelsVerbosity.Maximum);
        public static GremlinqOption<DisabledTextPredicates> DisabledTextPredicates = new GremlinqOption<DisabledTextPredicates>(Core.DisabledTextPredicates.None);
    }

    public class GremlinqOption<TValue> : GremlinqOption
    {
        public GremlinqOption(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; }
    }
}
