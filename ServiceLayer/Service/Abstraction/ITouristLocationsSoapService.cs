using System.ServiceModel;
using System.Xml.Linq;

namespace ServiceLayer.Service.Abstraction
{
    [ServiceContract]
    public interface ITouristLocationsSoapService
    {
        [OperationContract]
        Task<XDocument> GetXmlWithSearchedEntities(string term);
    }
}
