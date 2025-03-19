using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CnpjMailingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CnpjMailingApi.Controllers
{
    [Route("/api/[controller]")]
    public class CnpjMailingController : Controller
    {
        private readonly CnpjMailingService _service;

        public CnpjMailingController(CnpjMailingService service)
        {
            _service = service;
        }

        //TODO: Validar os campos de filtro para verificar se estão no formato correto.

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CnpjFilterDto filter)
        {

            List<string> cnaesPrimarios = filter.CnaesPrimarios.Split(',').ToList() ?? new List<string>();
            List<string> uf = filter.Ufs?.Split(',').ToList() ?? new List<string>();
            List<string> bairros = filter.Bairros?.Split(',').ToList() ?? new List<string>();
            List<string> municipios = filter.Municipios?.Split(',').ToList() ?? new List<string>();

            var cnpjs = await _service.GetCnpjsByFilter(
                cnaesPrimarios,
                filter.CnaeSecundario,
                filter.Identificador,
                uf,
                bairros,
                municipios,
                filter.OpcaoMei
            );

            if (filter.GerarCsv)
            {

                var csvContent = "CNPJ" + "\n" + string.Join("\n", cnpjs);
                return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", $"mailing_{DateTime.Now}.csv");
            }

            return Ok(cnpjs);

        }

    }
}