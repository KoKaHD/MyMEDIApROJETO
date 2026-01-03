using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        public ProdutoRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Produto>> GetAllAsync() =>
            await _context.Produtos.Include(p => p.Categoria).Include(p => p.ModoEntrega).ToListAsync();

        public async Task<Produto?> GetByIdAsync(int id) =>
            await _context.Produtos.Include(p => p.Categoria).Include(p => p.ModoEntrega).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId) =>
            await _context.Produtos.Where(p => p.CategoriaId == categoriaId).ToListAsync();

        public async Task AddAsync(Produto produto) { await _context.Produtos.AddAsync(produto); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Produto produto) { _context.Produtos.Update(produto); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var p = await GetByIdAsync(id); if (p != null) _context.Produtos.Remove(p); await _context.SaveChangesAsync(); }
    }
}
