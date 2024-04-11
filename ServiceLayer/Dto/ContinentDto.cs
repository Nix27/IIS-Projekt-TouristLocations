using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "continent")]
    public class ContinentDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }
}
