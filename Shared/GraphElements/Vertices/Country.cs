namespace ExRam.Gremlinq.Tests
{
    public class Country : Vertex
    {
        public Meta<string> Name { get; set; }
        public string[] Languages { get; set; }
        public string CountryCallingCode { get; set; }
    }
}
