using System.Text;
using CnpjMailingApi.Data;
using CnpjMailingApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using NpgsqlTypes;

namespace CnpjMailingApi.Repos
{
    //TODO: Tratar erro com CNPJ que não está sendo atribuído ao @NumeroCnpj
    public class CnpjDataRepository : ICnpjDataRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public CnpjDataRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<CnpjDataDto>> GetDataByCnpj(string cnpj)
        {
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.AppendLine(@"SELECT CONCAT(e.cnpj_basico, e.cnpj_ordem, e.cnpj_dv) AS CNPJ,
	emp.RAZAO_SOCIAL,
	e.nome_fantasia,
	emp.qualificacao,
	emp.natureza,
	emp.capital_social,
	emp.porte_empresa,
	e.identificador,
	e.situacao_cadastral,
	e.motivo,
	e.cnae_principal,
	e.cnae_secundario,
	e.tipo_logradouro,
	e.logradouro,
	e.numero,
	e.complemento,
	e.bairro,
	e.cep,
	e.uf,
	e.municipio,
	e.ddd_1,
	e.numero_1,
	e.ddd_2,
	e.numero_2,
	e.email
	FROM ESTABELECIMENTO e JOIN EMPRESAS emp ON e.CNPJ_BASICO = emp.CNPJ_BASICO
    WHERE CONCAT(e.cnpj_basico, e.cnpj_ordem, e.cnpj_dv) = @NumeroCnpj");

            var query = queryBuilder.ToString();

            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);

            if (!string.IsNullOrEmpty(cnpj))
            {
                command.Parameters.Add(new NpgsqlParameter("@NumeroCnpj", NpgsqlDbType.Varchar)
                {
                    Value = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace("%2F", "")
                });
            }
            Console.WriteLine($"cnpj enviado: {cnpj}");
            Console.WriteLine(query);

            command.CommandTimeout = 180;


            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new List<CnpjDataDto>()
                {
                    new CnpjDataDto()
                    {
                        Cnpj = reader.GetString(0),
                        RazaoSocial = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        NomeFantasia = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Qualificacao = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CapitalSocial = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        PorteEmpresa = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Identificador = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        SituacaoCadastral = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Motivo = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        CnaePrincipal = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        CnaeSecundario = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        TipoLogradouro = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Logradouro = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Numero = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Complemento = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Bairro = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        Cep = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        Uf = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        Municipio = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                        Ddd1 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Tel1 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                        Ddd2 = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                        Tel2 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                        Email = reader.IsDBNull(23) ? string.Empty : reader.GetString(23)
                    }
            };
            }
            else
            {
                return null;
            }

        }
    }
}