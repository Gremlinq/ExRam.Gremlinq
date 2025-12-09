using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generator
{
    [XmlRoot(ElementName = "node")]
    public class Node
    {
        [XmlElement(ElementName = "data")]
        public List<Data>? Data { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
