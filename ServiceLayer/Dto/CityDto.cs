using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "city")]
    public class CityDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "country")]
        public CountryDto Country { get; set; }
    }
}
