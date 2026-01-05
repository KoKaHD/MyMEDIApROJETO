using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;

namespace MyMEDIA.GestaoLoja.Services;

public class CategoriaBackofficeService
{
    private readonly ApplicationDbContext _db;
    public CategoriaBackofficeService(ApplicationDbContext db) => _db = db;

    public Task<List<Categoria>> ListAsync() =>
        _db.Categorias.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();

    public async Task AddAsync(string nome)
    {
        nome = nome.Trim();
        if (string.IsNullOrWhiteSpace(nome)) throw new InvalidOperationException("Nome obrigatório.");

        _db.Categorias.Add(new Categoria { Nome = nome });
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string nome)
    {
        var c = await _db.Categorias.FindAsync(id) ?? throw new InvalidOperationException("Categoria não encontrada.");
        c.Nome = nome.Trim();
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var temProdutos = await _db.Produtos.AnyAsync(p => p.CategoriaId == id);
        if (temProdutos) throw new InvalidOperationException("Não pode apagar: existem produtos nesta categoria.");

        var c = await _db.Categorias.FindAsync(id) ?? throw new InvalidOperationException("Categoria não encontrada.");
        _db.Categorias.Remove(c);
        await _db.SaveChangesAsync();
    }
}