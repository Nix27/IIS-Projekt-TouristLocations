using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "continent")]
    public class ContinentDto
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }
}
