using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMedia.Domain.Entities;

namespace MyMEDIA.GestaoLoja.Services;

public class VendasBackofficeService
{
    private readonly ApplicationDbContext _db;
    public VendasBackofficeService(ApplicationDbContext db) => _db = db;

    public async Task<(List<Encomenda> Items, int Total)> SearchAsync(
        string? estado, string? pagamento, int page, int pageSize)
    {
        var query = _db.Encomendas
            .AsNoTracking()
            .Include(e => e.Cliente)
            .Include(e => e.DetalhesEncomenda)
                .ThenInclude(d => d.Produto)
            .OrderByDescending(e => e.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(e => e.Estado == estado);

        if (!string.IsNullOrWhiteSpace(pagamento))
            query = query.Where(e => e.EstadoPagamento == pagamento);

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public async Task ConfirmarAsync(int encomendaId)
    {
        var e = await _db.Encomendas.FirstOrDefaultAsync(x => x.Id == encomendaId)
            ?? throw new InvalidOperationException("Encomenda não encontrada.");

        if (e.Estado != "Pendente")
            throw new InvalidOperationException("Só pode confirmar encomendas no estado Pendente.");

        e.Estado = "Confirmada";
        await _db.SaveChangesAsync();
    }

    public async Task RejeitarAsync(int encomendaId)
    {
        var e = await _db.Encomendas.FirstOrDefaultAsync(x => x.Id == encomendaId)
            ?? throw new InvalidOperationException("Encomenda não encontrada.");

        if (e.Estado != "Pendente")
            throw new InvalidOperationException("Só pode rejeitar encomendas no estado Pendente.");

        e.Estado = "Rejeitada";
        await _db.SaveChangesAsync();
    }

    public async Task MarcarPagoAsync(int encomendaId)
    {
        var e = await _db.Encomendas.FirstOrDefaultAsync(x => x.Id == encomendaId)
            ?? throw new InvalidOperationException("Encomenda não encontrada.");

        if (e.Estado == "Rejeitada")
            throw new InvalidOperationException("Encomenda rejeitada não pode ser paga.");

        e.EstadoPagamento = "Pago";
        e.DataPagamento = DateTime.Now;

        await _db.SaveChangesAsync();
    }

    public async Task ExpedirAsync(int encomendaId)
    {
        // transação para stock + estado
        await using var tx = await _db.Database.BeginTransactionAsync();

        var e = await _db.Encomendas
            .Include(x => x.DetalhesEncomenda)
            .ThenInclude(d => d.Produto)
            .FirstOrDefaultAsync(x => x.Id == encomendaId)
            ?? throw new InvalidOperationException("Encomenda não encontrada.");

        if (e.Estado != "Confirmada")
            throw new InvalidOperationException("Só pode expedir encomendas Confirmadas.");

        if (e.EstadoPagamento != "Pago")
            throw new InvalidOperationException("Só pode expedir encomendas Pagas.");

        if (e.Estado == "Expedida")
            throw new InvalidOperationException("Encomenda já expedida.");

        // validar e atualizar stock
        foreach (var d in e.DetalhesEncomenda)
        {
            if (d.Produto is null)
                throw new InvalidOperationException("Produto não encontrado num detalhe da encomenda.");

            if (d.Quantidade <= 0)
                throw new InvalidOperationException("Quantidade inválida num detalhe.");

            if (d.Produto.EmStock < d.Quantidade)
                throw new InvalidOperationException($"Stock insuficiente no produto #{d.Produto.Id} - {d.Produto.Nome}.");
        }

        foreach (var d in e.DetalhesEncomenda)
            d.Produto!.EmStock -= d.Quantidade;

        e.Estado = "Expedida";
        e.DataExpedicao = DateTime.Now;

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }
}