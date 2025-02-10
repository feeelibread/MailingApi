using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnpjMailingApi.Data;
using Npgsql;

namespace CnpjMailingApi.Repos
{
    public class CnpjMailingRepository : ICnpjMailingRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public CnpjMailingRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<string>> GetCnpjsByFilter(List<string> cnaes, List<string> cidades, List<string> municipios, string opcaoMei, List<string> bairro)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(@"SELECT CONCAT(e.CNPJ_BASICO, e.CNPJ_ORDEM, e.CNPJ_DV) AS CNPJ FROM ESTABELECIMENTO e
JOIN MUNICIPIOS m ON e.MUNICIPIO = m.CODIGO");

            if (!string.IsNullOrEmpty(opcaoMei))
            {
                queryBuilder.AppendLine("JOIN SIMPLES s ON e.CNPJ_BASICO = s.CNPJ_BASICO");
            }

            queryBuilder.AppendLine("WHERE e.SITUACAO_CADASTRAL = '02'");

            if (cnaes != null && cnaes.Any())
            {
                queryBuilder.AppendLine("AND e.CNAE_PRINCIPAL IN (" + string.Join(",", cnaes.Select(c => $"'{c}'")) + ")");
            }

            if (cidades != null && cidades.Any())
            {
                queryBuilder.AppendLine("AND e.UF IN (" + string.Join(",", cidades.Select(ci => $"'{ci}'")) + ")");
            }

            if (municipios != null && municipios.Any())
            {
                queryBuilder.AppendLine("AND m.DESCRICAO IN (" + string.Join(",", municipios.Select(m => $"'{m}'")) + ")");
            }

            if (bairro != null && bairro.Any())
            {
                queryBuilder.AppendLine("AND e.BAIRRO IN (" + string.Join(",", bairro.Select(b => $"'{b}'")) + ")");
            }

            if (!string.IsNullOrEmpty(opcaoMei))
            {
                queryBuilder.AppendLine("AND s.OPCAO_MEI = @OpcaoMei");
            }

            var query = queryBuilder.ToString();
            System.Console.WriteLine(query);

            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);
            if (!string.IsNullOrEmpty(opcaoMei))
                command.Parameters.AddWithValue("@OpcaoMei", opcaoMei);

            var cnpjs = new List<string>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cnpjs.Add(reader.GetString(0)); // Lê o campo CNPJ
            }

            return cnpjs;
        }

    }
}