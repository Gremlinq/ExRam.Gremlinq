using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generators
{
    [XmlRoot(ElementName = "graphml", Namespace = "http://graphml.graphdrawing.org/xmlns")]
    public class Graphml
    {
        [XmlElement(ElementName = "key")]
        public List<Key>? Key { get; set; }

        [XmlElement(ElementName = "graph")]
        public Graph? Graph { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string? Xmlns { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
