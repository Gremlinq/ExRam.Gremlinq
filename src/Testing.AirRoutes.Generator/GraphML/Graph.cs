using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generator
{
    [XmlRoot(ElementName = "graph")]
    public class Graph
    {
        [XmlElement(ElementName = "node")]
        public List<Node>? Node { get; set; }

        [XmlElement(ElementName = "edge")]
        public List<Edge>? Edge { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; set; }

        [XmlAttribute(AttributeName = "edgedefault")]
        public string? Edgedefault { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
