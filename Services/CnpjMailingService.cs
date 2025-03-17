using CnpjMailingApi.Repos;

namespace CnpjMailingApi.Services
{
    public class CnpjMailingService
    {
        private readonly ICnpjMailingRepository _repos;
        public CnpjMailingService(ICnpjMailingRepository repos)
        {
            _repos = repos;
        }

        public async Task<IEnumerable<string>> GetCnpjsByFilter(List<string> cnaesPrimarios, string? cnaeSecundario, string? identificador, List<string> cidade, List<string> bairros, List<string> municipios, string? opcaoMei)
        {
            return await _repos.GetCnpjsByFilter(cnaesPrimarios, cnaeSecundario, identificador, cidade, municipios, opcaoMei, bairros);
        }
    }
}