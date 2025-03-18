using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CnpjMailingApi.DTOs;
using CnpjMailingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql.Replication.PgOutput.Messages;

namespace CnpjMailingApi.Controllers
{
    [Route("api/[controller]")]
    public class CnpjDataController : Controller
    {
        private readonly CnpjDataService _service;
        public CnpjDataController(CnpjDataService service)
        {
            _service = service;
        }

        [HttpGet("{cnpj}")]
        public async Task<IActionResult> Get(string cnpj)
        {
            var result = await _service.GetDataByCnpj(cnpj);

            if (string.IsNullOrEmpty(cnpj))
            {
                return BadRequest("O campo não pode estar vazio");
            }
            
            if (cnpj.Length < 14)
            {
                return BadRequest("O CNPJ precisa ter 14 digitos.");
            }
            

            if (result == null)
            {
                return NotFound($"Nenhum dado encontrado para o CNPJ: {cnpj}");
            }

            return Ok(result);


        }

    }
}