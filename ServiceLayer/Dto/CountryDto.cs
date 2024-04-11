using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "country")]
    public class CountryDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "continent")]
        public ContinentDto Continent { get; set; }
    }
}
