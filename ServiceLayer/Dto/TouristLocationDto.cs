using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "touristLocation")]
    public class TouristLocationDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "rating")]
        public int Rating { get; set; }
        [XmlElement(ElementName = "city")]
        public CityDto City { get; set; }
    }
}
