using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<string>> GetCnpjsByFilter(List<string> cidade, List<string> cnaes, List<string> bairros, List<string> municipios, string opcaoMei)
        {
            return await _repos.GetCnpjsByFilter(cnaes, cidade, municipios, opcaoMei, bairros);
        }
    }
}