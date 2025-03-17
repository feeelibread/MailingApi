using CnpjMailingApi.DTOs;

namespace CnpjMailingApi.Repos
{
    public interface ICnpjDataRepository
    {
        Task<IEnumerable<CnpjDataDto>> GetDataByCnpj(string cnpj);
    }
}