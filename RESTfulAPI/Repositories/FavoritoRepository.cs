using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;
namespace RESTfulAPI.Repositories
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly ApplicationDbContext _context;
        public FavoritoRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Favorito>> GetByClienteAsync(string clienteId) =>
            await _context.Favoritos
                .Where(f => f.ClienteId == clienteId)
                .Include(f => f.Produto)
                .ToListAsync();

        public async Task<Favorito?> GetByClienteAndProdutoAsync(string clienteId, int produtoId) =>
            await _context.Favoritos
                .FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.ProdutoId == produtoId);

        public async Task AddAsync(Favorito favorito) { await _context.Favoritos.AddAsync(favorito); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var f = await _context.Favoritos.FindAsync(id); if (f != null) _context.Favoritos.Remove(f); await _context.SaveChangesAsync(); }
    }
}
