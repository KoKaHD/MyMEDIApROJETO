using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public interface ICarrinhoRepository
    {
        Task<IEnumerable<Carrinho>> GetByClienteAsync(string clienteId);
        Task<Carrinho?> GetByClienteAndProdutoAsync(string clienteId, int produtoId);
        Task AddAsync(Carrinho item);
        Task UpdateAsync(Carrinho item);
        Task DeleteAsync(int id);
        Task ClearByClienteAsync(string clienteId);
    }
}
