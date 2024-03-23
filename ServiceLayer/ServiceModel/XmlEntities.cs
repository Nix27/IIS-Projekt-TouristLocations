using System.Xml.Serialization;

namespace ServiceLayer.ServiceModel
{
    [XmlRoot(ElementName = "touristLocation")]
    public class XmlTouristLocation
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = null!;

        [XmlElement(ElementName = "description")]
        public string Description { get; set; } = null!;

        [XmlElement(ElementName = "rating")]
        public int Rating { get; set; }
    }

    [XmlRoot(ElementName = "touristLocations")]
    public class TouristLocations
    {

        [XmlElement(ElementName = "touristLocation")]
        public List<XmlTouristLocation> ListOfTouristLocations { get; set; } = null!;
    }

    [XmlRoot(ElementName = "city")]
    public class XmlCity
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = null!;

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "touristLocations")]
        public TouristLocations TouristLocations { get; set; } = null!;
    }

    [XmlRoot(ElementName = "cities")]
    public class Cities
    {

        [XmlElement(ElementName = "city")]
        public List<XmlCity> ListOfCities { get; set; } = null!;
    }

    [XmlRoot(ElementName = "country")]
    public class XmlCountry
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = null!;

        [XmlElement(ElementName = "officialLanguage")]
        public string OfficialLanguage { get; set; } = null!;

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "cities")]
        public Cities Cities { get; set; } = null!;
    }

    [XmlRoot(ElementName = "countries")]
    public class Countries
    {

        [XmlElement(ElementName = "country")]
        public List<XmlCountry> ListOfCountries { get; set; } = null!;
    }

    [XmlRoot(ElementName = "continent")]
    public class XmlContinent
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = null!;

        [XmlElement(ElementName = "population")]
        public int Population { get; set; }

        [XmlElement(ElementName = "countries")]
        public Countries Countries { get; set; } = null!;
    }

    [XmlRoot(ElementName = "continents")]
    public class Continents
    {

        [XmlElement(ElementName = "continent")]
        public List<XmlContinent> ListOfContinents { get; set; } = null!;
    }

    [XmlRoot(ElementName = "planet")]
    public class XmlPlanet
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = null!;

        [XmlElement(ElementName = "continents")]
        public Continents Continents { get; set; } = null!;
    }

    [XmlRoot(ElementName = "planets")]
    public class Planets
    {
        [XmlElement(ElementName = "planet")]
        public List<XmlPlanet> ListOfPlanets { get; set; } = null!;
    }
}
