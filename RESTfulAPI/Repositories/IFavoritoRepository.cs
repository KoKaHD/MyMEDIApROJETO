using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public interface IFavoritoRepository
    {
        Task<IEnumerable<Favorito>> GetByClienteAsync(string clienteId);
        Task<Favorito?> GetByClienteAndProdutoAsync(string clienteId, int produtoId);
        Task AddAsync(Favorito favorito);
        Task DeleteAsync(int id);
    }
}
