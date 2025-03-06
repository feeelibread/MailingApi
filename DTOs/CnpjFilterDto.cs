using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnpjMailingApi
{
    public class CnpjFilterDto
    {
        public string? Ufs { get; set; }
        public required string CnaesPrimarios { get; set; }
        public string? CnaeSecundario { get; set; }
        public string? Identificador { get; set; }
        public string? Bairros { get; set; }
        public string? Municipios { get; set; }
        public string? OpcaoMei { get; set; }
        public bool GerarCsv { get; set; }
    }
}