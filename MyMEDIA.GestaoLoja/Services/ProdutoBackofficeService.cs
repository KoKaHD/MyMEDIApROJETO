using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;

namespace MyMEDIA.GestaoLoja.Services;

public class ProdutoBackofficeService
{
    private readonly ApplicationDbContext _db;

    public ProdutoBackofficeService(ApplicationDbContext db) => _db = db;

    public async Task<(List<Produto> Items, int Total)> SearchAsync(
         string? q,
    int? categoriaId,
    int? modoEntregaId,
    EstadoProduto? estado,
    string? tipo,          
    string? orderBy,        
    int page, int pageSize)
    {
        var query = _db.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.ModoEntrega)
            .Include(p => p.Fornecedor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(p => p.Nome.Contains(q) || p.Titulo.Contains(q));
        }

        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId.Value);

        if (estado.HasValue)
            query = query.Where(p => p.Estado == estado.Value);
        if (modoEntregaId.HasValue)
            query = query.Where(p => p.ModoEntregaId == modoEntregaId.Value);

        if (!string.IsNullOrWhiteSpace(tipo))
        {
            query = tipo switch
            {
                "Venda" => query.Where(p => p.ParaVenda),
                "Listagem" => query.Where(p => !p.ParaVenda && p.ParaListagem),
                "Ambos" => query.Where(p => p.ParaVenda || p.ParaListagem),
                _ => query
            };
        }

        query = orderBy switch
        {
            "nome_asc" => query.OrderBy(p => p.Nome),
            "preco_asc" => query.OrderBy(p => p.PrecoFinal),
            "preco_desc" => query.OrderByDescending(p => p.PrecoFinal),
            "stock_desc" => query.OrderByDescending(p => p.EmStock),
            _ => query.OrderByDescending(p => p.Id)
        };
        /*query = query.OrderByDescending(p => p.Id);*/

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, total);
    }

    public Task<List<Categoria>> GetCategoriasAsync() =>
        _db.Categorias.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();
    public Task<List<ModoEntrega>> GetModosEntregaAsync() =>
     _db.ModosEntrega.AsNoTracking().OrderBy(m => m.Nome).ToListAsync();

    public async Task<string> GetFornecedorInternoIdAsync()
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == "fornecedor@interno.pt");

        if (user is null) throw new InvalidOperationException("FornecedorInterno não existe (seed).");
        return user.Id;
    }
    public async Task<int> CreateAsync(Produto p)
    {
        // Regra: produto criado no backoffice pode ficar Ativo ou Pendente (tu decides)
        /*if (p.Estado == 0) p.Estado = EstadoProduto.Ativo;*/

        // Se já tiver percentagem definida, calcula preço final
        p.PrecoFinal = Math.Round(p.PrecoBase * (1 + (p.PercentagemPlataforma / 100m)), 2);

        _db.Produtos.Add(p);
        await _db.SaveChangesAsync();
        return p.Id;
    }
    public async Task ValidateAsync(int produtoId, decimal percentagem)
    {
        var p = await _db.Produtos.FirstOrDefaultAsync(x => x.Id == produtoId)
            ?? throw new InvalidOperationException("Produto não encontrado.");

        if (percentagem < 0 || percentagem > 100)
            throw new InvalidOperationException("Percentagem inválida.");

        p.PercentagemPlataforma = percentagem;
        p.PrecoFinal = Math.Round(p.PrecoBase * (1 + (percentagem / 100m)), 2);
        p.Estado = EstadoProduto.Ativo;

        await _db.SaveChangesAsync();
    }

    public async Task SetEstadoAsync(int produtoId, EstadoProduto novoEstado)
    {
        var p = await _db.Produtos.FirstOrDefaultAsync(x => x.Id == produtoId)
            ?? throw new InvalidOperationException("Produto não encontrado.");

        p.Estado = novoEstado;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int produtoId)
    {
        var hasVendas = await _db.DetalhesEncomenda.AnyAsync(d => d.ProdutoId == produtoId);
        if (hasVendas)
            throw new InvalidOperationException("Não pode apagar: já existem vendas desse produto.");

        var p = await _db.Produtos.FirstOrDefaultAsync(x => x.Id == produtoId)
            ?? throw new InvalidOperationException("Produto não encontrado.");

        _db.Produtos.Remove(p);
        await _db.SaveChangesAsync();
    }
}