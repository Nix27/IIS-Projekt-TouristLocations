using System.Xml.Serialization;

namespace ServiceLayer.ServiceModel
{
    [XmlRoot(ElementName = "touristLocation")]
    public class TouristLocation
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "rating")]
        public int Rating { get; set; }
    }

    [XmlRoot(ElementName = "touristLocations")]
    public class TouristLocations
    {

        [XmlElement(ElementName = "touristLocation")]
        public List<TouristLocation> TouristLocation { get; set; }
    }

    [XmlRoot(ElementName = "city")]
    public class City
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "touristLocations")]
        public TouristLocations TouristLocations { get; set; }
    }

    [XmlRoot(ElementName = "cities")]
    public class Cities
    {

        [XmlElement(ElementName = "city")]
        public List<City> City { get; set; }
    }

    [XmlRoot(ElementName = "country")]
    public class Country
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "officialLanguage")]
        public string OfficialLanguage { get; set; }

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "cities")]
        public Cities Cities { get; set; }
    }

    [XmlRoot(ElementName = "countries")]
    public class Countries
    {

        [XmlElement(ElementName = "country")]
        public List<Country> Country { get; set; }
    }

    [XmlRoot(ElementName = "continent")]
    public class Continent
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "countries")]
        public Countries Countries { get; set; }
    }

    [XmlRoot(ElementName = "continents")]
    public class Continents
    {

        [XmlElement(ElementName = "continent")]
        public List<Continent> Continent { get; set; }
    }

    [XmlRoot(ElementName = "planet")]
    public class Planet
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "continents")]
        public Continents Continent { get; set; }
    }

    [XmlRoot(ElementName = "planets")]
    public class Planets
    {
        [XmlElement(ElementName = "planet")]
        public List<Planet> Planet { get; set; }
    }
}
