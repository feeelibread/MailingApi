namespace CnpjMailingApi.Repos
{
    public interface ICnpjMailingRepository
    {
        Task<IEnumerable<string>> GetCnpjsByFilter(List<string> cnaesPrimarios, string? cnaeSecundario, string? identificador, List<string> ufs, List<string> municipios, string? opcaoMei, List<string> bairros);
    }
}