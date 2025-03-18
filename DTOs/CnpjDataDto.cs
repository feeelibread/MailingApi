using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CnpjMailingApi.DTOs
{
    public class CnpjDataDto
    {
        public required string Cnpj { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Qualificacao { get; set; }
        public string? Natureza { get; set; }
        public string? CapitalSocial { get; set; }
        public string? PorteEmpresa { get; set; }
        public string? Identificador { get; set; }
        public string? SituacaoCadastral { get; set; }
        public string? Motivo { get; set; }
        public string? CnaePrincipal { get; set; }
        public string? CnaeSecundario { get; set; }
        public string? TipoLogradouro { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }
        public string? Uf { get; set; }
        public string? Municipio { get; set; }
        public string? Ddd1 { get; set; }
        public string? Tel1 { get; set; }
        public string? Ddd2 { get; set; }
        public string? Tel2 { get; set; }
        public string? Email { get; set; }
    }
}