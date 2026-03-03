using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generators
{
    [XmlRoot(ElementName = "key")]
    public class Key
    {
        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; set; }

        [XmlAttribute(AttributeName = "for")]
        public string? For { get; set; }

        [XmlAttribute(AttributeName = "attr.name")]
        public string? AttrName { get; set; }

        [XmlAttribute(AttributeName = "attr.type")]
        public string? AttrType { get; set; }
    }
}
