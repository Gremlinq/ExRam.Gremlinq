using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Country : Vertex
    {
        public Meta<string> Name { get; set; }
        public string[] Languages { get; set; }
        public string CountryCallingCode { get; set; }
    }
}
