using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;

namespace MyMEDIA.GestaoLoja.Services;

public class ModoEntregaBackofficeService
{
    private readonly ApplicationDbContext _db;
    public ModoEntregaBackofficeService(ApplicationDbContext db) => _db = db;

    public Task<List<ModoEntrega>> ListAsync() =>
        _db.ModosEntrega.AsNoTracking().OrderBy(m => m.Nome).ToListAsync();

    public async Task AddAsync(string nome)
    {
        nome = nome.Trim();
        if (string.IsNullOrWhiteSpace(nome)) throw new InvalidOperationException("Nome obrigatório.");

        _db.ModosEntrega.Add(new ModoEntrega { Nome = nome });
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string nome)
    {
        var m = await _db.ModosEntrega.FindAsync(id) ?? throw new InvalidOperationException("ModoEntrega não encontrado.");
        m.Nome = nome.Trim();
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var temProdutos = await _db.Produtos.AnyAsync(p => p.ModoEntregaId == id);
        if (temProdutos) throw new InvalidOperationException("Não pode apagar: existem produtos com este modo.");

        var m = await _db.ModosEntrega.FindAsync(id) ?? throw new InvalidOperationException("ModoEntrega não encontrado.");
        _db.ModosEntrega.Remove(m);
        await _db.SaveChangesAsync();
    }
}