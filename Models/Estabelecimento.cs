using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnpjMailingApi.Models
{
    public class Estabelecimento
    {
        public string? CnpjBasico { get; set; }
        public string? CnpjOrdem { get; set; }
        public string? CnpjDv { get; set; }
        public string? Identificador { get; set; }
        public string? SituacaoCadastral { get; set; }
        public string? CnaePrincipal { get; set; }
        public string? Bairro { get; set; }
        public string? Municipio { get; set; }
    }
}