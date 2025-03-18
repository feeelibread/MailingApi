using CnpjMailingApi.DTOs;

namespace CnpjMailingApi.Repos
{
    public interface ICnpjDataRepository
    {
        Task<CnpjDataDto> GetDataByCnpj(string cnpj);
    }
}