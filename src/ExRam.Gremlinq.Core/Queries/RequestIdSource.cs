namespace ExRam.Gremlinq.Core
{
    internal class RequestIdSource
    {
        internal static readonly RequestIdSource Default = new RequestIdSource(nameof(Default));

        internal RequestIdSource(string name = "unspecified")
        {
            Name = name;
        }

        public string Name { get; }
        internal Guid RequestId { get; set; }
    }
}
