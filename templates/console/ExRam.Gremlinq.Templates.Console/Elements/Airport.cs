namespace ExRam.Gremlinq.Templates.Console
{
#if (true) // --8<-- [start:airport]
    public sealed class Airport : Vertex
    {
        public string? Code { get; set; }
        public string? ICAO { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }

        public int Runways { get; set; }
        public int Elevation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int LongestRunway { get; set; }
    }
#endif // --8<-- [end:airport]
}
