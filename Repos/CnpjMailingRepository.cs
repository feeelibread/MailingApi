using System.Text;
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

        public async Task<IEnumerable<string>> GetCnpjsByFilter(List<string> cnaesPrimarios, string? cnaeSecundario, string? identificador, List<string> cidades, List<string> municipios, string? opcaoMei, List<string> bairro)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(@"SELECT CONCAT(e.CNPJ_BASICO, e.CNPJ_ORDEM, e.CNPJ_DV) AS CNPJ FROM ESTABELECIMENTO e
JOIN MUNICIPIOS m ON e.MUNICIPIO = m.CODIGO");

            if (!string.IsNullOrEmpty(opcaoMei))
            {
                queryBuilder.AppendLine("JOIN SIMPLES s ON e.CNPJ_BASICO = s.CNPJ_BASICO");
            }

            queryBuilder.AppendLine("WHERE e.SITUACAO_CADASTRAL = '02'");

            if (!string.IsNullOrEmpty(identificador))
            {
                queryBuilder.AppendLine("AND e.IDENTIFICADOR = @Identificador");
            }

            if (cnaesPrimarios != null && cnaesPrimarios.Any())
            {
                queryBuilder.AppendLine("AND e.CNAE_PRINCIPAL IN (" + string.Join(",", cnaesPrimarios.Select(c => $"'{c.Replace(".", "").Replace("-", "")}'")) + ")");
            }

            if (!string.IsNullOrEmpty(cnaeSecundario))
            {
                queryBuilder.AppendLine($"AND e.CNAE_SECUNDARIO LIKE @CnaeSecundario");
            }

            if (cidades != null && cidades.Any())
            {
                queryBuilder.AppendLine("AND e.UF IN (" + string.Join(",", cidades.Select(ci => $"'{ci.ToUpper()}'")) + ")");
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

            if (!string.IsNullOrEmpty(cnaeSecundario))
                command.Parameters.AddWithValue("@CnaeSecundario", $"%{cnaeSecundario.Replace(".", "").Replace("-", "")}%");

            if (!string.IsNullOrEmpty(identificador))
                command.Parameters.AddWithValue("@Identificador", identificador);


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