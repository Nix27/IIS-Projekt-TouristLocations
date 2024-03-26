using System.ServiceModel;

namespace ServiceLayer.Service.Abstraction
{
    [ServiceContract]
    public interface ITouristLocationsSoapService
    {
        [OperationContract]
        Task<string> GetSearchedEntities(string term);
    }
}
