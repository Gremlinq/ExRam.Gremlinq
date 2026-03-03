using System.Xml.Serialization;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generators
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlAttribute(AttributeName = "key")]
        public string? Key { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
