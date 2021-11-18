namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class GremlinqSetupInfo
    {
        public GremlinqSetupInfo(string? sectionName = null)
        {
            SectionName = sectionName;
        }

        public string? SectionName { get; }
    }
}
