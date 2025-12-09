using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generator
{
    [XmlRoot(ElementName = "edge")]
    public class Edge
    {
        [XmlElement(ElementName = "data")]
        public List<Data>? Data { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "source")]
        public int Source { get; set; }

        [XmlAttribute(AttributeName = "target")]
        public int Target { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
