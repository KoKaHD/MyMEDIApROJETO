using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories
{
    public class ModoEntregaRepository : IModoEntregaRepository
    {
        private readonly ApplicationDbContext _context;
        public ModoEntregaRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<ModoEntrega>> GetAllAsync() => await _context.ModosEntrega.ToListAsync();
        public async Task<ModoEntrega?> GetByIdAsync(int id) => await _context.ModosEntrega.FindAsync(id);
        public async Task AddAsync(ModoEntrega modo) { await _context.ModosEntrega.AddAsync(modo); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(ModoEntrega modo) { _context.ModosEntrega.Update(modo); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var m = await GetByIdAsync(id); if (m != null) _context.ModosEntrega.Remove(m); await _context.SaveChangesAsync(); }
    }
}
