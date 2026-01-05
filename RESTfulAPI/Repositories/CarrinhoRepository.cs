using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;
namespace RESTfulAPI.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly ApplicationDbContext _context;
        public CarrinhoRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Carrinho>> GetByClienteAsync(string clienteId) =>
            await _context.Carrinhos
                .Where(c => c.ClienteId == clienteId)
                .Include(c => c.Produto)
                .ToListAsync();

        public async Task<Carrinho?> GetByClienteAndProdutoAsync(string clienteId, int produtoId) =>
            await _context.Carrinhos
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.ProdutoId == produtoId);

        public async Task AddAsync(Carrinho item) { await _context.Carrinhos.AddAsync(item); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Carrinho item) { _context.Carrinhos.Update(item); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var c = await _context.Carrinhos.FindAsync(id); if (c != null) _context.Carrinhos.Remove(c); await _context.SaveChangesAsync(); }
        public async Task ClearByClienteAsync(string clienteId) => await _context.Carrinhos.Where(c => c.ClienteId == clienteId).ExecuteDeleteAsync();
    }
}
