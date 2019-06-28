namespace ExRam.Gremlinq.Core
{
    public struct ServerCapabilities
    {
        public ServerCapabilities(bool supportsTextPredicates)
        {
            SupportsTextPredicates = supportsTextPredicates;
        }

        public ServerCapabilities SetSupportsTextPredicates(bool value)
        {
            return new ServerCapabilities(
                supportsTextPredicates: value);
        }


        public bool SupportsTextPredicates { get; set; }
    }
}