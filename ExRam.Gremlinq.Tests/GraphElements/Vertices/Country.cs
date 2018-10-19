namespace ExRam.Gremlinq.Tests
{
    public class Country : Vertex
    {
        public string Id { get; set; }
        public Meta<string> Name { get; set; }
        public string CountryCallingCode { get; set; }
    }
}
