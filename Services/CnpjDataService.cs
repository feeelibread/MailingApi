using CnpjMailingApi.DTOs;
using CnpjMailingApi.Repos;

namespace CnpjMailingApi.Services
{
    public class CnpjDataService
    {
        private readonly ICnpjDataRepository _repos;
        public CnpjDataService(ICnpjDataRepository repos)
        {
            _repos = repos;
        }

        public async Task<CnpjDataDto> GetDataByCnpj(string cnpj)
        {
            return await _repos.GetDataByCnpj(cnpj);
        }

    }
}