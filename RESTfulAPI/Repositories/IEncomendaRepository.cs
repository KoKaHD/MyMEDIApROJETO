using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public interface IEncomendaRepository
    {
        Task<IEnumerable<Encomenda>> GetByClienteAsync(string clienteId);
        Task<Encomenda?> GetByIdAsync(int id);
        Task AddAsync(Encomenda encomenda);
        Task UpdateAsync(Encomenda encomenda);
    }
}
