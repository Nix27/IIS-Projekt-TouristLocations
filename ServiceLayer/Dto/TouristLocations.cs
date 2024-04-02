using System.Xml.Serialization;

namespace ServiceLayer.Dto
{
    [XmlRoot(ElementName = "touristLocations")]
    public class TouristLocations
    {
        [XmlElement(ElementName = "touristLocation")]
        public List<TouristLocationDto> ListOfTouristLocations { get; set; }
    }
}
