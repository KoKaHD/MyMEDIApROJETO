using MyMedia.Domain.Entities;
namespace RESTfulAPI.Repositories
{
    public interface IModoEntregaRepository
    {
        Task<IEnumerable<ModoEntrega>> GetAllAsync();
        Task<ModoEntrega?> GetByIdAsync(int id);
        Task AddAsync(ModoEntrega modo);
        Task UpdateAsync(ModoEntrega modo);
        Task DeleteAsync(int id);
    }
}
