using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public class EncomendaRepository : IEncomendaRepository
    {
        private readonly ApplicationDbContext _context;
        public EncomendaRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Encomenda>> GetByClienteAsync(string clienteId) =>
            await _context.Encomendas
                .Where(e => e.ClienteId == clienteId)
                .Include(e => e.DetalhesEncomenda)
                .ThenInclude(d => d.Produto)
                .ToListAsync();

        public async Task<Encomenda?> GetByIdAsync(int id) =>
            await _context.Encomendas
                .Include(e => e.DetalhesEncomenda)
                .ThenInclude(d => d.Produto)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task AddAsync(Encomenda encomenda)
        {
            await _context.Encomendas.AddAsync(encomenda);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Encomenda encomenda)
        {
            _context.Encomendas.Update(encomenda);
            await _context.SaveChangesAsync();
        }
    }
}
